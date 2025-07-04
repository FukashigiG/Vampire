using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHSpE", menuName = "Game Data/HSpE/Crush")]
public class HSpE_Crush : BaseHSpE
{
    public int crushBorder;

    public override void OnHitSpecialEffect(Base_MobStatus status, Vector2 posi)
    {
        if(status.hitPoint <= crushBorder) status.Die();
    }
}
