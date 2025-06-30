using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHSpE", menuName = "Game Data/HSpE/IgnoreDefence")]
public class HSpE_IgnoreDefence : BaseHSpE
{
    public override bool IgnoreDefence
    {
        get { return true; } 
    }

    public override void OnHitSpecialEffect(Base_MobStatus status)
    {

    }
}
