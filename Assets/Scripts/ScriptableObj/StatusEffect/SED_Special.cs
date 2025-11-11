using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBuffEffect", menuName = "Game Data/StatusEffects/Special")]
public class SED_Special : Base_StatusEffectData
{
    enum Type { actable, arrowDamage, arrowHit, blaze, regene}

    [SerializeField] Type targetType;

    public override void Apply(Base_MobStatus target, int amount)
    {
        switch (targetType)
        {
            case Type.actable:
                target.actable = false;
                break;

            case Type.arrowDamage:
                target.isArrowDamage = false; 
                break;

            case Type.arrowHit:
                target.isArrowHit = false;
                break;

            case Type.blaze:
                target.damageOverTime = true;
                break;

            case Type.regene:
                target.onRegeneration = true;
                break;
        }
    }

    public override void Remove(Base_MobStatus target, int amount)
    {
        switch (targetType)
        {
            case Type.actable:
                if (! target.IsStatusEffectTypeActive(this)) target.actable = true;
                break;

            case Type.arrowDamage:
                if (! target.IsStatusEffectTypeActive(this)) target.isArrowDamage = true;
                break;

            case Type.arrowHit:
                if (! target.IsStatusEffectTypeActive(this)) target.isArrowHit = true;
                break;

            case Type.blaze:
                if (!target.IsStatusEffectTypeActive(this)) target.damageOverTime = false;
                break ;

            case Type.regene:
                if (!target.IsStatusEffectTypeActive(this)) target.onRegeneration = false;
                break;

        }
    }
}
