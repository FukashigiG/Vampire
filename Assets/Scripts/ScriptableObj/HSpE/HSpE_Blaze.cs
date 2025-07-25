using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHSpE", menuName = "Game Data/HSpE/Blaze")]
public class HSpE_Blaze : BaseHSpE
{
    
    // 当たった敵を火炎状態にする

    public float duration;

    public override void OnHitSpecialEffect(Base_MobStatus status, Vector2 posi, KnifeData knifeData)
    {
        status.ApplyStatusEffect(StatusEffectType.Blaze, duration);
    }
}
