using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Threading;
using System.Linq;

public class EnemySpawner : SingletonMono<EnemySpawner>
{
    [SerializeField] Transform parent_Enemy;
    [SerializeField] GameObject effect_DeleteEnemy;

    [SerializeField] GameObject bossEnemy;

    [SerializeField] float interval_Spawn;

    CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    CancellationToken token;

    // 購読のライフサイクルを管理するためのDisposable
    // これ１つで沢山のDisposableなやつらに対応可能らしい
    private CompositeDisposable disposables = new CompositeDisposable();

    List<EnemyData> normalEnemyList;

    int count_Die;

    void Start()
    {
        token = _cancellationTokenSource.Token;

        // エディタ上に登録された通常敵のリストを取得
        var x = Resources.LoadAll<EnemyData>("GameDatas/Enemy/Normal");
        normalEnemyList = new List<EnemyData>(x);

        count_Die = 0;

        SpawnTask(token).Forget();
    }

    async UniTask SpawnTask(CancellationToken token)
    {
        try
        {
            while (true)
            {
                Vector2 randomPoint = SpawnPointRottery();

                var x = Instantiate(EnemyLottery(), randomPoint, Quaternion.identity, parent_Enemy);

                var enemy = x.GetComponent<EnemyStatus>();

                // ウエーブ数の倍率ブーストを渡したうえでの初期化
                // enemy.Initialize(1 + (GameAdmin.Instance.waveCount - 1) * GameAdmin.Instance.waveBoostMultiplier);
                // 仕様変更：初期化はステータス自身にやらせる

                // ミニマップ管理人に、新しく生まれたことを知らせる
                //MiniMapController.Instance.NewEnemyInstance(enemy);
                // 仕様変更：これもステータス自身にやらせる

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
        int[] weight_Rank = { 50, 20, 5 };

        int sum = 75;

        int randomPoint = Random.Range(1, sum + 1);

        int cullent = 0;
        EnemyData spawnTargetData = null;

        for (int i = 0; i < weight_Rank.Length; i++)
        {
            cullent += weight_Rank[i];

            if (cullent >= randomPoint)
            {
                var targetList = normalEnemyList
                            .Where(x => x.rank == i + 1)
                            .ToList();

                spawnTargetData = targetList[Random.Range(0, targetList.Count)];

                break;
            }
        }

        

        return spawnTargetData.prefab;
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

        GameObject x = Instantiate(bossEnemy, randomPoint, Quaternion.identity, parent_Enemy);

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
