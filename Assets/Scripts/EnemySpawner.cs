using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Threading;

public class EnemySpawner : SingletonMono<EnemySpawner>
{
    [SerializeField] Transform parent_Enemy;
    [SerializeField] GameObject effect_DeleteEnemy;

    [SerializeField] GameObject[] enemy;
    [SerializeField] GameObject bossEnemy;

    [SerializeField] float interval_Spawn;

    CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    CancellationToken token;

    // �w�ǂ̃��C�t�T�C�N�����Ǘ����邽�߂�Disposable
    // ����P�ő�R��Disposable�Ȃ��ɑΉ��\�炵��
    private CompositeDisposable disposables = new CompositeDisposable();

    int count_Die;

    void Start()
    {
        token = _cancellationTokenSource.Token;

        SpawnTask(token).Forget();

        EnemyStatus.onDie.Subscribe(x => CountMobDie()).AddTo(disposables);

        count_Die = 0;
    }

    async UniTask SpawnTask(CancellationToken token)
    {
        try
        {
            while (true)
            {
                Vector2 randomPoint = SpawnPointRottery();

                var x = Instantiate(EnemyLottery(), randomPoint, Quaternion.identity, parent_Enemy);

                // �E�G�[�u���̔{���u�[�X�g��n���������ł̏�����
                x.GetComponent<EnemyStatus>().Initialize(1 + (GameAdmin.Instance.waveCount - 1) * GameAdmin.Instance.waveBoostMultiplier);

                await UniTask.Delay((int)(interval_Spawn * 1000), cancellationToken: token);

                // �U�R���E�F�[�u�܂ő҂�
                await UniTask.WaitUntil(() => GameAdmin.Instance._waveState == GameAdmin.WaveState.zako, PlayerLoopTiming.Update, token);
            }
        }
        catch
        {

        }
        finally
        {

        }
    }

    Vector2 SpawnPointRottery()
    {
        Vector2 randomPoint;

        Transform player = PlayerController.Instance.transform;

        float n = 10;

        // �X�|�[���n�_���v���C���[�Ɣ��an�ȓ��ɂȂ�Ȃ��悤��
        do
        {
            randomPoint = Random.insideUnitCircle * 15;

        } while (Vector2.Distance(randomPoint, player.position) < n);

        return randomPoint;
    }

    GameObject EnemyLottery()
    {
        int x = Random.Range(0, enemy.Length);

        return enemy[x];
    }

    void CountMobDie()
    {
        count_Die++;

        if(count_Die >= 10)
        {
            SpawnBoss();

            count_Die = 0;
        }
    }

    // �����̓G��S�폜
    public void DestroyAllEnemies()
    {
        foreach(Transform x in parent_Enemy)
        {
            Instantiate(effect_DeleteEnemy, x.transform.position, Quaternion.identity);

            Destroy(x.gameObject);
        }
    }

    public GameObject SpawnBoss()
    {
        Vector2 randomPoint = SpawnPointRottery();

        var x = Instantiate(bossEnemy, randomPoint, Quaternion.identity, parent_Enemy);

        // �E�G�[�u���̔{���u�[�X�g��n���������ł̏�����
        x.GetComponent<EnemyStatus>().Initialize(1 + GameAdmin.Instance.waveCount * GameAdmin.Instance.waveBoostMultiplier);

        return x;
    }

    private void OnDestroy()
    {
        disposables.Dispose();

        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
    }
}
