using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewFieldLogic", menuName = "Game Data/FieldLogic/Buff")]
public class FEL_Buff : Base_FieldEffectLogic
{
    [Serializable] enum EffectType { atk, def, spd}

    [SerializeField] EffectType effectType = EffectType.spd;

    public override void OnApplyEffect(Base_MobStatus status)
    {
        switch (effectType)
        {
            case EffectType.atk:
                status.enhancementRate_Power += 100;
                break;

            case EffectType.def:
                status.enhancementRate_Defence += 100;
                break;

            case EffectType.spd:
                status.enhancementRate_MoveSpeed += 100;
                break;
        }

        
    }

    public override void OnRemoveEffect(Base_MobStatus status)
    {
        switch(effectType)
        {
            case EffectType.atk:
                status.enhancementRate_Power -= 100;
                break;

            case EffectType.def:
                status.enhancementRate_Defence -= 100;
                break;

            case EffectType.spd:
                status.enhancementRate_MoveSpeed -= 100;
                break;
        }
    }

    public override void OnSecondEffect(Base_MobStatus status, int effectPoint, Vector2 fieldCenter)
    {
        
    }
}
