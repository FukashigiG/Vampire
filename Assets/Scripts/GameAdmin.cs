using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class GameAdmin : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject panel_LvUp;

    [SerializeField] int num_Wave;

    bool onGame;

    // 購読のライフサイクルを管理するためのDisposable
    CompositeDisposable disposables = new CompositeDisposable();

    void Start()
    {
        onGame = true;

        player.GetComponent<PlayerStatus>().lvUp.Subscribe(_ => ShowLevelUpUIAsync().Forget()).AddTo( disposables );

        GameProgression().Forget();
    }

    //全体的なゲームの進行を管理
    async UniTask GameProgression()
    {
        for (int i = 0; i < num_Wave; i++)
        {
            await UniTask.Delay((int)(5000));
        }

        onGame = false;
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

    private void OnDestroy()
    {
        disposables.Dispose();
    }
}
