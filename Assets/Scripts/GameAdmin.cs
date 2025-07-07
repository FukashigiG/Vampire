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
    [SerializeField] GameObject player_Obj;
    [SerializeField] GameObject spawner_Obj;

    [SerializeField] GameObject panel_LvUp;

    [SerializeField] Text txt_WaveCount;
    [SerializeField] Text txt_TimeLimit_Wave;

    [SerializeField] int num_Wave;
    [SerializeField] float minute_Wave;

    CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    CancellationToken _cancellationToken;

    bool onGame;

    EnemySpawner _spawner;

    public int waveCount = 0;

     // �P�E�F�[�u������̓G�̋����{��
    public float waveBoostMultiplier {  get; private set; }
    [SerializeField] float _waveBoostMultiplier;

    public enum WaveState
    {
        zako, boss
    }

    public WaveState _waveState;

    // �w�ǂ̃��C�t�T�C�N�����Ǘ����邽�߂�Disposable
    CompositeDisposable disposables = new CompositeDisposable();

    void Start()
    {
        _cancellationToken = _cancellationTokenSource.Token;

        onGame = true;

        waveBoostMultiplier = _waveBoostMultiplier;

        player_Obj.GetComponent<PlayerStatus>().lvUp.Subscribe(_ => ShowLevelUpUIAsync().Forget()).AddTo( disposables );

        _spawner = spawner_Obj.GetComponent<EnemySpawner>();

        GameProgression().Forget();
    }

    //�S�̓I�ȃQ�[���̐i�s���Ǘ�
    async UniTask GameProgression()
    {
        while (true)
        {
            waveCount++;

            txt_WaveCount.text = "Wave : " + waveCount;

            // �E�F�[�u�̏�ԕϐ��̍X�V
            _waveState = WaveState.zako;

            //�E�F�[�u�̎��ԑ҂�
            await WaitWithWave(minute_Wave, _cancellationToken);
            // �L�����Z���ς݂��`�F�b�N
            _cancellationToken.ThrowIfCancellationRequested();

            // UI�X�V�𐳂����s�����߂�1�t���҂�
            await UniTask.Yield(PlayerLoopTiming.Update);

            // �����̓G��S����
            _spawner.DestroyAllEnemies();

            // �{�X����
            var x = _spawner.SpawnBoss();

            if (txt_TimeLimit_Wave != null) txt_TimeLimit_Wave.text = "�{�X�o��";

            // �E�F�[�u�̏�ԕϐ��̍X�V
            _waveState = WaveState.boss;

            // �{�X�����܂ő҂�
            await UniTask.WaitUntil(() => x == null, PlayerLoopTiming.Update, _cancellationToken);
            _cancellationToken.ThrowIfCancellationRequested();

            if (txt_TimeLimit_Wave != null) txt_TimeLimit_Wave.text = "�{�X���j";

            // 1�b�҂�
            await UniTask.Delay(1000, ignoreTimeScale: false, PlayerLoopTiming.Update, _cancellationToken);
            _cancellationToken.ThrowIfCancellationRequested();
        }
    }

    async UniTask WaitWithWave(float min, CancellationToken token)
    {
        float sec = min * 60;
        float remainingTime = sec;

        // UI��Text�𒼐ڍX�V���铽���֐�
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
                // 1�t���҂�
                // "PlayerLoopTiming.Update"������Ɓu�X�V�^�C�~���O��Unity��UpDate�֐��ɍ��킹��v
                //�i������Update�O�ɏ��������j
                await UniTask.Yield(PlayerLoopTiming.Update, token);
                token.ThrowIfCancellationRequested();

                // �c�莞�Ԃ̍X�V
                remainingTime -= Time.deltaTime;

                // �i�s�󋵂��
                progress.Report(remainingTime);
            }
        }
        catch(OperationCanceledException)
        {
            // ��O����
        }
        finally
        {
            // �Ō��
        }
    }

    async UniTask ShowLevelUpUIAsync()
    {
        if (panel_LvUp.activeSelf) return;

        panel_LvUp.SetActive(true);

        PauseGame();

        await UniTask.WaitUntil(() => panel_LvUp.activeSelf == false);

        ResumeGame();
    }

    // �ꎞ��~
    void PauseGame()
    {
        Time.timeScale = 0f;
    }

    // �ĊJ
    void ResumeGame()
    {
        Time.timeScale = 1f;
    }

    //GameAdmin�̏������A�܂�Q�[���V�[���I�����̏���
    private void OnDestroy()
    {
        disposables.Dispose();

        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();

        Base_MobStatus.onDie.Dispose();
        Button_AddKnifeCtrler.clicked.Dispose();
    }
}
