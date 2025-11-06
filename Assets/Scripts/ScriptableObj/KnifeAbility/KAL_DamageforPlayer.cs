using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewKnifeAbility", menuName = "Game Data/KnifeAbility/DamageforPlayer")]
public class KAL_DamageForPlayer : Base_KnifeAbilityLogic
{
    // “Š±ƒvƒŒƒCƒ„[‚ÍUŒ‚‚ğó‚¯‚é

    public override void ActivateEffect_OnThrown(PlayerStatus status, Vector2 posi, KnifeData_RunTime knifeData, float modifire)
    {
        base.ActivateEffect_OnThrown (status, posi, knifeData, modifire);

        status.GetAttack(knifeData.power / 3, status.transform.position);
    }
}
