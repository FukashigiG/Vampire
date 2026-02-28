using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBossEnemyAct", menuName = "Game Data/BossEnemyAct/Summon")]
public class BEA_Summon : Base_BossEnemyAct
{
    [SerializeField] GameObject warningPrefab;
    [SerializeField] GameObject fx_Summon;
    [SerializeField] EnemyData summonedEnemyData;

    [SerializeField] int num_summonEnemy;

    float spawnRadius = 2.4f;

    public async override UniTask Action(EnemyCtrler_BigBoss ctrler, CancellationToken token)
    {
        Vector2 center = ctrler.transform.position;

        Vector2[] spawnPosies = new Vector2[num_summonEnemy];

        for (int i = 0; i < num_summonEnemy; i++)
        {
            float angle = i * 360f / num_summonEnemy;

            float radian = angle * Mathf.Deg2Rad;

            spawnPosies[i] = center + new Vector2(Mathf.Cos(radian), Mathf.Sin(radian)) * spawnRadius;
        }

        List<GameObject> warningObj = new List<GameObject>();

        foreach (Vector2 spawnPos in spawnPosies)
        {
            // 警告オブジェクトを生成
            GameObject warning = Instantiate(warningPrefab, spawnPos, Quaternion.identity);

            warningObj.Add(warning);

            // 初期化、これらは個別にまたない
            warning.GetComponent<EP_Warning>().WarningAnim(delayTime, token, AttackRangeType.circle, 0, size_Range: 1.5f).Forget();
        }

        ctrler._animator.SetTrigger("Spell");

        try
        {
            await UniTask.Delay((int)(delayTime * 1000), cancellationToken: token);
        }
        catch
        {
            foreach(var warning in warningObj) Destroy(warning);

            throw;
        }

        foreach (var pos in spawnPosies)
        {
            Instantiate(fx_Summon, pos, Quaternion.identity);
            EnemySpawner.Instance.SpawnEnemy(summonedEnemyData, pos);
        }

        await UniTask.Delay(1000 * 2, cancellationToken: token);
    }
}
