using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHSpE", menuName = "Game Data/HSpE/speedDebuff")]
public class HSpE_SpeedDebuff: BaseHSpE
{
    public float dulation;
    public int amount_percent;

    public override void OnHitSpecialEffect(Base_MobStatus status, Vector2 posi, KnifeData knifeData)
    {
        status.ApplyStatusEffect(StatusEffectType.MoveSpeed, dulation, -1 * amount_percent);
    }
}
