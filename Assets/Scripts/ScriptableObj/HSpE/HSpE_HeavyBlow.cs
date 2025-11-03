using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHSpE", menuName = "Game Data/HSpE/HeavyBlow")]
public class HSpE_HeavyBlow : BaseHSpE
{
    public float knockPower;

    protected override void ActivateEffect(Base_MobStatus status, Vector2 posi, KnifeData_RunTime knifeData)
    {
        status.KnockBack(posi, knockPower);

        base.ActivateEffect(status, posi, knifeData);
    }
}
