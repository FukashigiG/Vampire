using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBossEnemyAct", menuName = "Game Data/BossEnemyAct/Basic/Anger")]
public class BEA_Anger : Base_BossEnemyAct
{
    [SerializeField] GameObject prefab_Fx;

    public async override UniTask Action(EnemyCtrler_BigBoss ctrler, CancellationToken token)
    {
        ctrler._enemyStatus.enhancementRate_Power += 30;
        ctrler._enemyStatus.enhancementRate_MoveSpeed += 20;

        ctrler._animator.SetTrigger("Spell");

        if(prefab_Fx != null) Instantiate(prefab_Fx, (Vector2)ctrler.transform.position + Vector2.up, Quaternion.identity, ctrler.transform);

        await UniTask.Delay((int)(1000 * 1.75f), cancellationToken: token);
    }
}
