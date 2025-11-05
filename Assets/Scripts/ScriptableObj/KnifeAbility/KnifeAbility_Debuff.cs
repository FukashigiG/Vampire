using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewKnifeAbility", menuName = "Game Data/KnifeAbility/debuff")]
public class KnifeAbility_Debuff : Base_KnifeAbility
{
    // ヒットした相手にデバフ

    public float dulation;
    public int amount_percent;
    [SerializeField] string effectID;
    [SerializeField] StatusEffectType targetState;

    protected override void ActivateEffect_OnHit(Base_MobStatus status, Vector2 posi, KnifeData_RunTime knifeData)
    {
        status.ApplyStatusEffect(targetState, effectID, dulation, amount_percent);

    }
}
