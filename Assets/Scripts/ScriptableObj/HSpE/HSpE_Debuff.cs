using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHSpE", menuName = "Game Data/HSpE/debuff")]
public class HSpE_Debuff : BaseHSpE
{
    public float dulation;
    public int amount_percent;
    [SerializeField] string effectID;
    [SerializeField] StatusEffectType targetState;

    public override void OnHitSpecialEffect(Base_MobStatus status, Vector2 posi, KnifeData_RunTime knifeData)
    {
        status.ApplyStatusEffect(targetState, effectID, dulation, -1 * amount_percent);
    }
}
