using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class GameAdmin : MonoBehaviour
{
    [SerializeField] GameObject player_Obj;
    [SerializeField] GameObject spawner_Obj;

    [SerializeField] GameObject panel_LvUp;

    [SerializeField] Text txt_TimeLimit_Wave;

    [SerializeField] int num_Wave;
    [SerializeField] float minute_Wave;

    CancellationTokenSource _cancellationTokenSource;

    bool onGame;

    EnemySpawner _spawner;

    // �w�ǂ̃��C�t�T�C�N�����Ǘ����邽�߂�Disposable
    CompositeDisposable disposables = new CompositeDisposable();

    void Start()
    {
        onGame = true;

        player_Obj.GetComponent<PlayerStatus>().lvUp.Subscribe(_ => ShowLevelUpUIAsync().Forget()).AddTo( disposables );

        _spawner = spawner_Obj.GetComponent<EnemySpawner>();

        GameProgression().Forget();
    }

    //�S�̓I�ȃQ�[���̐i�s���Ǘ�
    async UniTask GameProgression()
    {
        for (int i = 0; i < num_Wave; i++)
        {
            //�E�F�[�u�̎��ԑ҂�
            await WaitWithWave(minute_Wave);

            //�{�X����
            var x = _spawner.SpawnBoss();

            // �{�X�����܂ő҂�
            await UniTask.WaitUntil(() => x == null);
        }

        // �X�e�[�W�N���A
        onGame = false;
    }

    async UniTask WaitWithWave(float min)
    {
        float sec = min * 60;
        float remainingTime = sec;

        // UI��Text�𒼐ڍX�V���铽���֐�
        IProgress<float> progress = new Progress<float>(value =>
        {
            TimeSpan ts = TimeSpan.FromSeconds(value);
            txt_TimeLimit_Wave.text = $"{ts.Minutes:00}:{ts.Seconds:00}";
        });

        try
        {
            while(remainingTime > 0)
            {
                await UniTask.Yield();
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

    void PauseGame()
    {
        Time.timeScale = 0f;
    }

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
