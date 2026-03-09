using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewKnifeAbility", menuName = "Game Data/KnifeAbility/ReleaseEA")]
public class KAL_ReleaseEnemyAbility : Base_KnifeAbilityLogic
{
    // ヒットした敵のアビリティを解除

    public override void ActivateAbility(Base_MobStatus status, GameObject knifeObject, KnifeData_RunTime knifeData, string effectID)
    {
        base.ActivateAbility(status, knifeObject, knifeData, effectID);

        if(status is EnemyStatus enemy)
        {
            if(enemy._enemyData.actType == EnemyData.EnemyActType.BigBoss) return;

            enemy.CancelAbilities();
        }
    }
}
