using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHSpE", menuName = "Game Data/HSpE/penetration")]
public class HSpE_SpeedDebuff: BaseHSpE
{
    public float dulation;
    public float amount;

    public override void OnHitSpecialEffect(Base_MobStatus status)
    {
        status.MoveSpeedDebuff(dulation, amount);
    }
}
