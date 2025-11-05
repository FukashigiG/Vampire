using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewKnifeAbility", menuName = "Game Data/KnifeAbility/HeavyBlow")]
public class KnifeAbility_HeavyBlow : Base_KnifeAbility
{
    public float knockPower;

    protected override void ActivateEffect_OnHit(Base_MobStatus status, Vector2 posi, KnifeData_RunTime knifeData)
    {
        status.KnockBack(posi, knockPower);
    }
}
