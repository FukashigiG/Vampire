using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewKnifeAbility", menuName = "Game Data/KnifeAbility/debuff")]
public class KAL_Debuff : Base_KnifeAbilityLogic
{
    // ëŒè€Ç…èÛë‘ïœâª

    [SerializeField] float dulation;
    [SerializeField] int amount_percent;

    [SerializeField] Base_StatusEffectData statusEffect;

    public override void ActivateAbility(Base_MobStatus status, GameObject knifeObj, KnifeData_RunTime knifeData, float modifire, string effectID)
    {
        base.ActivateAbility(status, knifeObj, knifeData, modifire, effectID);

        status.ApplyStatusEffect(statusEffect, effectID, dulation * modifire, amount_percent);
    }
}
