using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewKnifeAbility", menuName = "Game Data/KnifeAbility/DamageforPlayer")]
public class KAL_DamageForPlayer : Base_KnifeAbilityLogic
{
    // 投擲時プレイヤーはダメージを受ける

    public override void ActivateAbility(Base_MobStatus status, GameObject knifeObj, KnifeData_RunTime knifeData, float modifire, string effectID)
    {
        base.ActivateAbility (status, knifeObj, knifeData, modifire, effectID);

        status.TakeDamage(1 * (int)modifire);
    }
}
