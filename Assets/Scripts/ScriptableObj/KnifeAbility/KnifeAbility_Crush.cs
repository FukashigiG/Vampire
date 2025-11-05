using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewKnifeAbility", menuName = "Game Data/KnifeAbility/Crush")]
public class KnifeAbility_Crush : Base_KnifeAbility
{
    // ƒqƒbƒg‚µ‚½“G‚ð‘¦Ž€‚³‚¹‚é

    protected override void ActivateEffect_OnHit(Base_MobStatus status, Vector2 posi, KnifeData_RunTime knifeData)
    {
        status.Die();
    }
}
