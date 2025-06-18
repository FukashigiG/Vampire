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

    // �w�ǂ̃��C�t�T�C�N�����Ǘ����邽�߂�Disposable
    CompositeDisposable disposables = new CompositeDisposable();

    void Start()
    {
        onGame = true;

        player.GetComponent<PlayerStatus>().lvUp.Subscribe(_ => ShowLevelUpUIAsync().Forget()).AddTo( disposables );

        GameProgression().Forget();
    }

    //�S�̓I�ȃQ�[���̐i�s���Ǘ�
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
