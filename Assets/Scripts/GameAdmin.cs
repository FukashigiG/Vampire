using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UniRx;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameAdmin : SingletonMono<GameAdmin>
{
    [SerializeField] GameObject panel_LvUp;

    [SerializeField] TextMeshProUGUI txt_WaveCount;
    [SerializeField] TextMeshProUGUI txt_TimeLimit_Wave;

    [SerializeField] CinemachineCamera v_Camera_FocusOnBoss;

    [SerializeField] int num_Wave;
    [SerializeField] float minute_Wave;

    [SerializeField] StageData initialStage;

    [SerializeField] GameObject item_WarpStage;

    [SerializeField] StageDataHolder dataHolder;

    CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    CancellationToken _cancellationToken;

    public int waveCount { get; private set; } = 0;

    List<StageData> _stageHistory = new List<StageData>();
    public IReadOnlyList<StageData> stageHistory => _stageHistory;

    public StageData currentStage { get; private set; }

    int pauseCount = 0;

    public bool isPausing => pauseCount > 0;

    EnemyStatus cullentBoss = null;

    // １ウェーブあたりの敵の強化倍率
    [field: SerializeField] public float waveBoostMultiplier {  get; private set; }

    float cullentTimeScale = 1f;

    public enum WaveState
    {
        zako, boss
    }
    public WaveState _waveState;

    public bool isEndLess {  get; private set; } = false;

    // ボス出現時の通知
    Subject<Unit> subject_OnBossAppear = new Subject<Unit>();
    public IObservable<Unit> onBossAppear => subject_OnBossAppear;

    // 購読のライフサイクルを管理するためのDisposable
    CompositeDisposable disposables = new CompositeDisposable();

    protected override void Awake()
    {
        base.Awake();

        Time.timeScale = 1.0f;

        _cancellationToken = _cancellationTokenSource.Token;

        currentStage = initialStage;

        var p_Status = PlayerController.Instance.GetComponent<PlayerStatus>();

        var charaData = dataHolder.selectedChara;

        isEndLess = dataHolder.isEndless;

        p_Status.Initialize_OR(charaData);

        p_Status.onDie.Subscribe(x =>
        {
            GameSet(false);

        }).AddTo(this);
    }

    public void UpdateWave(StageData stageData)
    {
        WaveProgression().Forget(); // 先にこれを実行しておかないと、waveStateが更新されずスポナーが機能しない

        _stageHistory.Add(stageData);

        EnemySpawner.Instance.SetEnemies(stageData.enemyList, stageData.bossEnemy);

        GameEventDirector.Instance.SetEvents(stageData.eventList);

        UI_ShowStageName.Instance.SetStageInfo(waveCount, stageData.stageName);

        StageGroundCtrler.Instance.ChangeGroundImg(stageData.groungSprite);
    }

    //全体的なゲームの進行を管理
    async UniTask WaveProgression()
    {
        waveCount++;

        txt_WaveCount.text = "Wave : " + waveCount;

        // ウェーブの状態変数の更新
        _waveState = WaveState.zako;

        //ウェーブの時間待つ
        await WaitWithWave(minute_Wave, _cancellationToken);
        // キャンセル済みかチェック
        _cancellationToken.ThrowIfCancellationRequested();

        // UI更新を正しく行うための1フレ待ち
        await UniTask.Yield(PlayerLoopTiming.Update);

        // 既存の敵を全除去
        EnemySpawner.Instance.Stop_SpawnTask();

        await BossAppear();

        // ボス討伐通知を受け取るまで待つ
        await cullentBoss.onDie
            .First()
            .ToUniTask(cancellationToken: _cancellationToken);

        await OnBossDefeated(cullentBoss.gameObject);

        // ウェーブ終了時の処理
        OnWaveFinish();
    }

    async UniTask WaitWithWave(float min, CancellationToken token)
    {
        float sec = min * 60;
        float remainingTime = sec;

        // UIのTextを直接更新する匿名関数
        IProgress<float> progress = new Progress<float>(value =>
        {
            //if (txt_TimeLimit_Wave == null) return;

            TimeSpan ts = TimeSpan.FromSeconds(value);
            txt_TimeLimit_Wave.text = $"{ts.Minutes:00}:{ts.Seconds:00}";
        });

        try
        {
            while(remainingTime > 0)
            {
                // 1フレ待つ
                // "PlayerLoopTiming.Update"をつけると「更新タイミングをUnityのUpDate関数に合わせる」
                //（処理はUpdate前に処理される）
                await UniTask.Yield(PlayerLoopTiming.Update, token);
                token.ThrowIfCancellationRequested();

                // 残り時間の更新
                remainingTime -= Time.deltaTime;

                // 進行状況を報告
                progress.Report(remainingTime);
            }
        }
        catch(OperationCanceledException)
        {
            // 例外処理
        }
        finally
        {
            // 最後に
        }
    }

    // ボス出現処理
    async UniTask BossAppear()
    {
        // 通知を飛ばす
        subject_OnBossAppear.OnNext(Unit.Default);

        // テキストの更新
        if (txt_TimeLimit_Wave != null) txt_TimeLimit_Wave.text = "ボス出現！";

        // ウェーブの状態変数の更新
        _waveState = WaveState.boss;

        // 一瞬のディレイ
        await UniTask.Delay(500, cancellationToken: _cancellationToken);

        var spawnPos = EnemySpawner.Instance.SpawnPointRottery();

        // ボス注目カメラを、ボス出現演出間はオンに
        v_Camera_FocusOnBoss.transform.position = (Vector3)spawnPos + new Vector3(0, 0, -10);
        v_Camera_FocusOnBoss.gameObject.SetActive(true);

        // カメラ切り替わり完了まで待つ
        float blendTime = Camera.main.GetComponent<CinemachineBrain>().DefaultBlend.BlendTime;
        await UniTask.Delay((int)((blendTime + 0.5f) * 1000), cancellationToken: _cancellationToken);


        // ボス生成
        cullentBoss = EnemySpawner.Instance.SpawnBoss(spawnPos);

        UI_BossHPGauge.Instance.Initialize(cullentBoss.gameObject);

        var _animator = cullentBoss.GetComponent<Animator>();

        // アニメーション偏移を待機するための処理
        await UniTask.Yield();

        // 登場モーション終了まで待つ
        await UniTask.WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);

        // ボス注目カメラを切る
        v_Camera_FocusOnBoss.gameObject.SetActive(false);
    }

    // ボスが死んだとき
    async UniTask OnBossDefeated(GameObject bossObj)
    {
        if (txt_TimeLimit_Wave != null) txt_TimeLimit_Wave.text = "ボス撃破";

        // ボス注目カメラを、ボス死亡演出間はオンに
        v_Camera_FocusOnBoss.Target.TrackingTarget = bossObj.transform;
        v_Camera_FocusOnBoss.gameObject.SetActive(true);

        SetTimeScaleValue(0.5f);

        try
        {
            // ボスのオブジェクトがなくなるまで待機
            await UniTask.WaitUntil(() => bossObj == null, cancellationToken: _cancellationToken);

            // 画面を揺らす
            var source = GetComponent<CinemachineImpulseSource>();
            source.GenerateImpulse();

            // 少し待つ
            await UniTask.Delay((int)(1.5f * 1000 * Time.timeScale), cancellationToken: _cancellationToken);
        }
        finally
        {
            SetTimeScaleValue(1f);

            v_Camera_FocusOnBoss.gameObject.SetActive(false);
        }
    }

    void OnWaveFinish()
    {
        if(! isEndLess && waveCount >= 6)
        {
            GameSet(true);

            return;
        }

        Vector2 P_pos = PlayerController.Instance.transform.position;

        // ゲートの場所
        // プレイヤーの現在地から原点方向に４先の位置
        Vector2 posi = P_pos + (Vector2.zero - P_pos).normalized * 4;
        // ワープゲート生成
        Instantiate(item_WarpStage, posi, Quaternion.identity);
    }

    public void SetTimeScaleValue(float x)
    {
        if(x <= 0) return;

        cullentTimeScale = x;
        Time.timeScale = cullentTimeScale;
    }

    // 一時停止
    public void PauseGame()
    {
        pauseCount++;

        Time.timeScale = 0f;
    }

    // 再開
    public void ResumeGame()
    {
        pauseCount--;

        if (pauseCount == 0)
        {
            Time.timeScale = cullentTimeScale;
        }
    }

    public void ReTry()
    {
        SceneLoader.Instance.Load("TitleScene");
    }

    public void GameSet(bool isPlayerWin)
    {
        UI_GameResult.Instance.OnGameSet(isPlayerWin);

        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
        _cancellationTokenSource = null;
    }

    //GameAdminの消失時、つまりゲームシーン終了時の処理
    private void OnDestroy()
    {
        disposables.Dispose();

        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
        _cancellationTokenSource = null;
    }
}
