using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHSpE", menuName = "Game Data/HSpE/speedDebuff")]
public class HSpE_SpeedDebuff: BaseHSpE
{
    public float dulation;
    public float amount;

    public override void OnHitSpecialEffect(Base_MobStatus status, Vector2 posi, KnifeData knifeData)
    {
        status.MoveSpeedDebuff(dulation, amount);
    }
}
