using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewKnifeAbility", menuName = "Game Data/KnifeAbility/VanishOnThrow")]
public class KAL_VanishOnThrow : Base_KnifeAbilityLogic
{
    // è¡ñ≈Ç∑ÇÈ

    public override void ActivateAbility(Base_MobStatus status, GameObject knifeObj, KnifeData_RunTime knifeData, float modifire, string effectID)
    {
        base.ActivateAbility (status, knifeObj, knifeData, modifire, effectID);

        Destroy(knifeObj);
    }
}
