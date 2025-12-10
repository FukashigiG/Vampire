using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBossEnemyAct", menuName = "Game Data/BossEnemyAct/Summon")]
public class BEA_Summon : Base_BossEnemyAct
{
    [SerializeField] GameObject fx_Summon;
    [SerializeField] EnemyData summonedEnemyData;
    [SerializeField] int num_summonEnemy;

    float spawnRadius = 2.4f;

    public async override UniTask Action(Base_EnemyCtrler ctrler, CancellationToken token)
    {
        Vector2 center = ctrler.transform.position;

        Vector2[] spawnPosies = new Vector2[num_summonEnemy];

        for (int i = 0; i < num_summonEnemy; i++)
        {
            float angle = i * 360f / num_summonEnemy;

            float radian = angle * Mathf.Deg2Rad;

            spawnPosies[i] = center + new Vector2(Mathf.Cos(radian), Mathf.Sin(radian)) * spawnRadius;
        }

        foreach (var pos in spawnPosies)
        {
            Instantiate(fx_Summon, pos, Quaternion.identity);
            EnemySpawner.Instance.SpawnEnemy(summonedEnemyData, pos);
        }

        await UniTask.Delay(1000 * 2, cancellationToken: token);
    }
}
