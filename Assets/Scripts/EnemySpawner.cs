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

    // 購読のライフサイクルを管理するためのDisposable
    // これ１つで沢山のDisposableなやつらに対応可能らしい
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

                // ウエーブ数の倍率ブーストを渡したうえでの初期化
                x.GetComponent<EnemyStatus>().Initialize(1 + (GameAdmin.Instance.waveCount - 1) * GameAdmin.Instance.waveBoostMultiplier);

                await UniTask.Delay((int)(interval_Spawn * 1000), cancellationToken: token);

                // ザコ狩りウェーブまで待つ
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

        // スポーン地点がプレイヤーと半径n以内にならないように
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

    // 現存の敵を全削除
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

        // ウエーブ数の倍率ブーストを渡したうえでの初期化
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
