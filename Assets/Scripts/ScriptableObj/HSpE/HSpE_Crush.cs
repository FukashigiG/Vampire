using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHSpE", menuName = "Game Data/HSpE/Crush")]
public class HSpE_Crush : BaseHSpE
{
    // ƒqƒbƒg‚µ‚½“G‚ð‘¦Ž€‚³‚¹‚é

    protected override void ActivateEffect(Base_MobStatus status, Vector2 posi, KnifeData_RunTime knifeData)
    {
        base.ActivateEffect(status, posi, knifeData);

        status.Die();
    }
}
