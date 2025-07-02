using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHSpE", menuName = "Game Data/HSpE/attackDebuff")]
public class HSpE_AttackDebuff : BaseHSpE
{
    public float dulation;
    public float amount;

    public override void OnHitSpecialEffect(Base_MobStatus status, Vector2 posi)
    {
        status.AttackDebuff(dulation, amount);
    }
}
