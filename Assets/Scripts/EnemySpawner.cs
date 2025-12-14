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

    [SerializeField] GameObject prefab_NormalEnemy_Infight;
    [SerializeField] GameObject prefab_NormalEnemy_Shooter;
    [SerializeField] GameObject prefab_NormalEnemy_Fielder;

    [SerializeField] GameObject bossEnemy;
    

    [SerializeField] float interval_Spawn;

    CancellationTokenSource tokenSource = new CancellationTokenSource();
    CancellationToken token;

    // 購読のライフサイクルを管理するためのDisposable
    // これ１つで沢山のDisposableなやつらに対応可能らしい
    private CompositeDisposable disposables = new CompositeDisposable();

    List<EnemyData> normalEnemyList = new List<EnemyData>();
    EnemyData bossData;

    void Awake()
    {
        token = tokenSource.Token;
    }

    public void SetEnemies(List<EnemyData> list, EnemyData _bossData)
    {
        normalEnemyList.Clear();

        normalEnemyList = new List<EnemyData>(list);

        bossData = _bossData;

        SpawnTask(token).Forget();
    }

    async UniTask SpawnTask(CancellationToken token)
    {
        try
        {
            while (GameAdmin.Instance._waveState == GameAdmin.WaveState.zako)
            {
                Vector2 randomPoint = SpawnPointRottery(); // 敵の生成場所を取得

                var data = EnemyLottery(); // 敵のデータを取得

                SpawnEnemy(data, randomPoint); // 生成

                // ミニマップ管理人に、新しく生まれたことを知らせる
                //MiniMapController.Instance.NewEnemyInstance(enemy);
                // 仕様変更：ステータス自身にやらせる

                float interval = 0.4f + (4 - GameAdmin.Instance.currentStage.stageRank) * 0.3f;

                // 待つ
                await UniTask.Delay((int)(interval * 1000), cancellationToken: token);
            }
        }
        catch (System.OperationCanceledException)
        {
            // キャンセルされたのでループを抜ける（ログは出さない）
        }
        catch (System.Exception e)
        {
            Debug.LogException(e); // エラー内容をログに出す
        }
        finally
        {

        }
    }

    // 与えられたデータと座標を元に敵オブジェクトを生成、初期化
    // 外部が新たに敵を出したい場合にもこれを利用させる
    public void SpawnEnemy(EnemyData data, Vector2 spawnPoint)
    {
        GameObject targetPrefab = null;

        // 敵のタイプにあわせたプレハブを取得
        // それぞれことなるタイプのエネミーコントローラがアタッチされてる
        switch (data.actType)
        {
            case EnemyData.EnemyActType.Infight:
                targetPrefab = prefab_NormalEnemy_Infight;
                break;

            case EnemyData.EnemyActType.Shooter:
                targetPrefab = prefab_NormalEnemy_Shooter;
                break;

            case EnemyData.EnemyActType.Fielder:
                targetPrefab = prefab_NormalEnemy_Fielder;
                break;
        }

        // 生成
        var x = Instantiate(targetPrefab, spawnPoint, Quaternion.identity, parent_Enemy);

        // データとウエーブ数の倍率ブーストを渡したうえで初期化させる
        x.GetComponent<EnemyStatus>()
        .Initialize(data, 1 + (GameAdmin.Instance.waveCount - 1) * GameAdmin.Instance.waveBoostMultiplier);
    }

    // 生成場所の抽選
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

    // 出現させる敵のデータを抽選
    EnemyData EnemyLottery()
    {
        int[] weight_Rank = { 9, 7, 4 };

        int sum = 0;

        foreach(var enemy in normalEnemyList)
        {
            sum += weight_Rank[enemy.rank - 1];
        }

        int randomPoint = Random.Range(1, sum + 1);

        int cullent = 0;
        EnemyData spawnTargetData = null;

        foreach (var enemy in normalEnemyList)
        {
            cullent += weight_Rank[enemy.rank - 1];

            if (cullent >= randomPoint)
            {
                spawnTargetData = enemy;

                break;
            }
        }

        return spawnTargetData;
    }

    public void Stop_SpawnTask()
    {
        tokenSource.Cancel();
        tokenSource.Dispose();
        tokenSource = new CancellationTokenSource();
        token = tokenSource.Token;

        foreach (Transform x in parent_Enemy)
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
        x.GetComponent<EnemyStatus>().Initialize(bossData, 1 + GameAdmin.Instance.waveCount * GameAdmin.Instance.waveBoostMultiplier);

        UI_BossHPGauge.Instance.Initialize(x);

        return x;
    }

    private void OnDestroy()
    {
        disposables.Dispose();
    }
}
