using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHSpE", menuName = "Game Data/HSpE/attackDebuff")]
public class HSpE_AttackDebuff : BaseHSpE
{
    public float dulation;
    public int amount_percent;
    [SerializeField] string effectID;

    public override void OnHitSpecialEffect(Base_MobStatus status, Vector2 posi, KnifeData_RunTime knifeData)
    {
        status.ApplyStatusEffect(StatusEffectType.Power, effectID, dulation, -1 * amount_percent);
    }
}
