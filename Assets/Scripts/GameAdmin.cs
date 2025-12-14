using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class GameAdmin : SingletonMono<GameAdmin>
{
    [SerializeField] GameObject panel_LvUp;

    [SerializeField] Text txt_WaveCount;
    [SerializeField] Text txt_TimeLimit_Wave;

    [SerializeField] int num_Wave;
    [SerializeField] float minute_Wave;

    [SerializeField] StageData initialStage;

    [SerializeField] GameObject item_WarpStage;

    CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    CancellationToken _cancellationToken;

    public int waveCount { get; private set; } = 0;

    public StageData currentStage { get; private set; }

    int pauseCount = 0;

     // １ウェーブあたりの敵の強化倍率
    [field: SerializeField] public float waveBoostMultiplier {  get; private set; }

    public enum WaveState
    {
        zako, boss
    }

    public WaveState _waveState;

    // 購読のライフサイクルを管理するためのDisposable
    CompositeDisposable disposables = new CompositeDisposable();

    private void Awake()
    {
        Time.timeScale = 1.0f;

        _cancellationToken = _cancellationTokenSource.Token;

        currentStage = initialStage;
    }

    private void Start()
    {
        PlayerController.Instance._status.onDie
            .Where(x => x.status == PlayerController.Instance._status)
            .Subscribe(x =>
            {
                GameOver();

            }).AddTo(this);
    }

    public void UpdateWave(StageData stageData)
    {
        WaveProgression().Forget(); // 先にこれを実行しておかないと、waveStateが更新されずスポナーが機能しない

        EnemySpawner.Instance.SetEnemies(stageData.enemyList, stageData.bossEnemy);

        GameEventDirector.Instance.SetEvents(stageData.eventList);
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

        // ボス生成
        var x = EnemySpawner.Instance.SpawnBoss();

        if (txt_TimeLimit_Wave != null) txt_TimeLimit_Wave.text = "ボス出現";

        // ウェーブの状態変数の更新
        _waveState = WaveState.boss;

        // ボス討伐まで待つ
        await UniTask.WaitUntil(() => x == null, PlayerLoopTiming.Update, _cancellationToken);
        _cancellationToken.ThrowIfCancellationRequested();

        if (txt_TimeLimit_Wave != null) txt_TimeLimit_Wave.text = "ボス撃破";

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

    void OnWaveFinish()
    {
        switch (waveCount)
        {
            case <= 5:
                // ワープゲート生成
                Instantiate(item_WarpStage, Vector2.zero, Quaternion.identity);
                break;

            default:
                break;
        }

        
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
            Time.timeScale = 1f;
        }
    }

    public void ReTry_Kari()
    {
        SceneLoader.Instance.Load("TitleScene");
    }

    void GameOver()
    {
        PauseGame();

        PlayerDiedPanelDirector.Instance.ShowPanel();
    }

    //GameAdminの消失時、つまりゲームシーン終了時の処理
    private void OnDestroy()
    {
        disposables.Dispose();

        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
    }
}
