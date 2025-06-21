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

    // 購読のライフサイクルを管理するためのDisposable
    CompositeDisposable disposables = new CompositeDisposable();

    void Start()
    {
        onGame = true;

        player_Obj.GetComponent<PlayerStatus>().lvUp.Subscribe(_ => ShowLevelUpUIAsync().Forget()).AddTo( disposables );

        _spawner = spawner_Obj.GetComponent<EnemySpawner>();

        GameProgression().Forget();
    }

    //全体的なゲームの進行を管理
    async UniTask GameProgression()
    {
        for (int i = 0; i < num_Wave; i++)
        {
            //ウェーブの時間待つ
            await WaitWithWave(minute_Wave);

            //ボス生成
            var x = _spawner.SpawnBoss();

            // ボス討伐まで待つ
            await UniTask.WaitUntil(() => x == null);
        }

        // ステージクリア
        onGame = false;
    }

    async UniTask WaitWithWave(float min)
    {
        float sec = min * 60;
        float remainingTime = sec;

        // UIのTextを直接更新する匿名関数
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

    //GameAdminの消失時、つまりゲームシーン終了時の処理
    private void OnDestroy()
    {
        disposables.Dispose();

        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();

        Base_MobStatus.onDie.Dispose();
        Button_AddKnifeCtrler.clicked.Dispose();
    }
}
