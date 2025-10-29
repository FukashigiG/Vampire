using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHSpE", menuName = "Game Data/HSpE/debuff")]
public class HSpE_Debuff : BaseHSpE
{
    enum targetStateEnum
    {
        power, diffence, speed, blaze, freeze
    }

    public float dulation;
    public int amount_percent;
    [SerializeField] string effectID;
    [SerializeField] targetStateEnum targetState;

    public override void OnHitSpecialEffect(Base_MobStatus status, Vector2 posi, KnifeData knifeData)
    {
        

        switch(targetState)
        {
            case targetStateEnum.power:

                status.ApplyStatusEffect(StatusEffectType.Power, effectID, dulation, -1 * amount_percent);
                break;

            case targetStateEnum.diffence:

                status.ApplyStatusEffect(StatusEffectType.Defence, effectID, dulation, -1 * amount_percent);
                break;

            case targetStateEnum.speed:

                status.ApplyStatusEffect(StatusEffectType.MoveSpeed, effectID, dulation, -1 * amount_percent);
                break;

            case targetStateEnum.blaze:

                status.ApplyStatusEffect(StatusEffectType.Blaze, effectID, dulation);
                break;

            case targetStateEnum.freeze:

                status.ApplyStatusEffect(StatusEffectType.Freeze, effectID, dulation);
                break;
        }
    }
}
