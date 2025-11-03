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

    protected override void ActivateEffect(Base_MobStatus status, Vector2 posi, KnifeData_RunTime knifeData)
    {
        Debug.Log("HSpE");

        status.ApplyStatusEffect(targetState, effectID, dulation, amount_percent);

        base.ActivateEffect(status, posi, knifeData);
    }
}
