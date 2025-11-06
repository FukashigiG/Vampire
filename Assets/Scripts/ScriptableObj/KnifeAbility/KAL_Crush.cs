using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewKnifeAbility", menuName = "Game Data/KnifeAbility/Crush")]
public class KAL_Crush : Base_KnifeAbilityLogic
{
    // ƒqƒbƒg‚µ‚½“G‚ð‘¦Ž€‚³‚¹‚é

    public override void ActivateEffect_OnHit(Base_MobStatus status, Vector2 posi, KnifeData_RunTime knifeData, float modifire)
    {
        base.ActivateEffect_OnHit (status, posi, knifeData, modifire);

        status.Die();
    }
}
