using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHSpE", menuName = "Game Data/HSpE/HeavyBlow")]
public class HSpE_HeavyBlow : BaseHSpE
{
    public float knockPower;

    public override void OnHitSpecialEffect(Base_MobStatus status, Vector2 posi, KnifeData knifeData)
    {
        status.KnockBack(posi, knockPower);
    }
}
