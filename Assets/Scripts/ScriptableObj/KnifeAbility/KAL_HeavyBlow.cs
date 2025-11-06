using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewKnifeAbility", menuName = "Game Data/KnifeAbility/HeavyBlow")]
public class KAL_HeavyBlow : Base_KnifeAbilityLogic
{
    [SerializeField] float knockPower;

    public override void ActivateEffect_OnHit(Base_MobStatus status, Vector2 posi, KnifeData_RunTime knifeData, float modifire)
    {
        base.ActivateEffect_OnHit(status, posi, knifeData, modifire);

        status.KnockBack(posi, knockPower);
    }
}
