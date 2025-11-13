using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewKnifeAbility", menuName = "Game Data/KnifeAbility/ChargeCharaAbility")]
public class KAL_ChargeCharaAbility : Base_KnifeAbilityLogic
{
    // 投擲時、追加でいくらかアビリティチャージ

    [SerializeField] int chargeAmount;

    public override void ActivateAbility(Base_MobStatus status, GameObject knifeObject, KnifeData_RunTime knifeData, string effectID)
    {
        base.ActivateAbility(status, knifeObject, knifeData, effectID);

        if(status is PlayerStatus player)
        {
            player.attack.AbilityCharge(chargeAmount);
        }
    }
}
