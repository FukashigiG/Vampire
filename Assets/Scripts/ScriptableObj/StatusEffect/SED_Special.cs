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
                target.count_Actable++;
                break;

            case Type.arrowDamage:
                target.count_PermissionDamage++;
                break;

            case Type.arrowHit:
                target.count_PermissionHit.Value++;
                break;

            case Type.blaze:
                target.count_PermissionDamageOverTime++;
                break;

            case Type.regene:
                target.count_PermissionRegeneration++;
                break;
        }
    }

    public override void Remove(Base_MobStatus target, int amount)
    {
        switch (targetType)
        {
            case Type.actable:
                target.count_Actable--;
                break;

            case Type.arrowDamage:
                target.count_PermissionDamage--;
                break;

            case Type.arrowHit:
                target.count_PermissionHit.Value--;
                break;

            case Type.blaze:
                target.count_PermissionDamageOverTime--;
                break;

            case Type.regene:
                target.count_PermissionRegeneration--;
                break;

        }
    }
}
