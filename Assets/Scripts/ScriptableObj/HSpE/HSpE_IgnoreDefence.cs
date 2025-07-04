using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHSpE", menuName = "Game Data/HSpE/IgnoreDefence")]
public class HSpE_IgnoreDefence : BaseHSpE
{
    // IgnoreDefence��true�ɏ㏑�����邱�ƂŖh�䖳��������
    public override bool ignoreDefence
    {
        get { return true; } 
    }

    public override void OnHitSpecialEffect(Base_MobStatus status, Vector2 p)
    {

    }
}
