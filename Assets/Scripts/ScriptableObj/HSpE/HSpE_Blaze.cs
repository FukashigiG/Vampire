using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHSpE", menuName = "Game Data/HSpE/Blaze")]
public class HSpE_Blaze : BaseHSpE
{
    public float duration;

    public override void OnHitSpecialEffect(Base_MobStatus status, Vector2 p)
    {
        status.Blaze(duration);
    }
}
