using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class GameAdmin : MonoBehaviour
{
    [SerializeField] GameObject player_Obj;
    [SerializeField] GameObject spawner_Obj;

    [SerializeField] GameObject panel_LvUp;

    [SerializeField] int num_Wave;
    [SerializeField] float minute_Wave;

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
            //2���҂�
            await UniTask.Delay((int)(60 * minute_Wave * 1000));

            var x = _spawner.SpawnBoss();

            Debug.Log(i);

            await UniTask.WaitUntil(() => x == null);
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

    //GameAdmin�̏������A�܂�Q�[���V�[���I�����̏���
    private void OnDestroy()
    {
        disposables.Dispose();

        Base_MobStatus.onDie.Dispose();
        Button_AddKnifeCtrler.clicked.Dispose();
    }
}
