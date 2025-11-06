using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewKnifeAbility", menuName = "Game Data/KnifeAbility/HeavyBlow")]
public class KAL_HeavyBlow : Base_KnifeAbilityLogic
{
    [SerializeField] float knockPower;

    public override void ActivateAbility(Base_MobStatus status, GameObject knifeObj, KnifeData_RunTime knifeData, float modifire, string effectID)
    {
        base.ActivateAbility(status, knifeObj, knifeData, modifire, effectID);

        status.KnockBack(knifeObj.transform.position, knockPower);
    }
}
