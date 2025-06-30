using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHSpE", menuName = "Game Data/HSpE/penetration")]
public class HSpE_Penetration : BaseHSpE
{
    public override bool DestroyBullet
    {
        get { return false; }
    }

    public override void OnHitSpecialEffect(Base_MobStatus status)
    {

    }
}
