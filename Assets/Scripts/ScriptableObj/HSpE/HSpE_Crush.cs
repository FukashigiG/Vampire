using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHSpE", menuName = "Game Data/HSpE/Crush")]
public class HSpE_Crush : BaseHSpE
{
    public float crushBorder;

    public override void OnHitSpecialEffect(Base_MobStatus status, Vector2 posi, KnifeData knifeData)
    {
        float d = status.hitPoint / status.maxHP;

        if (d <= crushBorder) status.Die();
    }
}
