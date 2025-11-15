using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewKnifeAbility", menuName = "Game Data/KnifeAbility/Heal")]
public class KAL_Heal : Base_KnifeAbilityLogic
{
    // 投擲時、HPをNパーセント回復

    [SerializeField] int ratio_Heal;

    public override void ActivateAbility(Base_MobStatus status, GameObject knifeObject, KnifeData_RunTime knifeData, string effectID)
    {
        base.ActivateAbility(status, knifeObject, knifeData, effectID);

        status.HealHP((int)(status.maxHP * ratio_Heal / 100f));
    }
}
