using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewKnifeAbility", menuName = "Game Data/KnifeAbility/Crush")]
public class KAL_Crush : Base_KnifeAbilityLogic
{
    // ƒqƒbƒg‚µ‚½“G‚ð‘¦Ž€‚³‚¹‚é

    public override void ActivateAbility(Base_MobStatus status, GameObject knifeObj, KnifeData_RunTime knifeData, float modifire, string effectID)
    {
        base.ActivateAbility(status, knifeObj, knifeData, modifire, effectID);

        status.Die();
    }
}
