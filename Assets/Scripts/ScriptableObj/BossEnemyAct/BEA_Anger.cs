using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBossEnemyAct", menuName = "Game Data/BossEnemyAct/Basic/Anger")]
public class BEA_Anger : Base_BossEnemyAct
{
    public async override UniTask Action(EnemyCtrler_BigBoss ctrler, CancellationToken token)
    {
        ctrler._enemyStatus.enhancementRate_Power += 40;
        ctrler._enemyStatus.enhancementRate_MoveSpeed += 20;

        ctrler._animator.SetTrigger("Spell");

        await UniTask.Delay(1000 * 2, cancellationToken: token);
    }
}
