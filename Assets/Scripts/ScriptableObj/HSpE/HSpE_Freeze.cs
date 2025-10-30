using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHSpE", menuName = "Game Data/HSpE/Freeze")]
public class HSpE_Freeze : BaseHSpE
{
    public float duration;

    public override void OnHitSpecialEffect(Base_MobStatus status, Vector2 posi, KnifeData_RunTime knifeData)
    {
        status.ApplyStatusEffect(StatusEffectType.Freeze, "tekitou", duration);
    }
}
