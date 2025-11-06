using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewKnifeAbility", menuName = "Game Data/KnifeAbility/debuff")]
public class KAL_Debuff : Base_KnifeAbilityLogic
{
    // ヒットした相手にデバフ

    [SerializeField] float dulation;
    [SerializeField] int amount_percent;
    [SerializeField] string effectID;
    [SerializeField] StatusEffectType targetState;

    public override void ActivateEffect_OnHit(Base_MobStatus status, Vector2 posi, KnifeData_RunTime knifeData, float modifire)
    {
        base.ActivateEffect_OnHit (status, posi, knifeData, modifire);

        status.ApplyStatusEffect(targetState, effectID, dulation, amount_percent);
    }
}
