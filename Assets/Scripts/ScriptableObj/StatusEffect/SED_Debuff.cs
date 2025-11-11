using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBuffEffect", menuName = "Game Data/StatusEffects/Debuff")]
public class SED_Debuff : Base_StatusEffectData
{
    enum Type { atk, def, spd, eye, luk}

    [SerializeField] Type targetType;

    public override void Apply(Base_MobStatus target, int amount)
    {
        switch (targetType)
        {
            case Type.atk:
                target.enhancementRate_Power -= amount;
                break;

            case Type.def:
                target.enhancementRate_Defence -= amount;
                break;

            case Type.spd:
                target.enhancementRate_MoveSpeed -= amount;
                break;

            case Type.eye:
                if (target is PlayerStatus player) player.enhancementRate_EyeSight -= amount;
                break;

            case Type.luk:
                if (target is PlayerStatus _player) _player.enhancementRate_Luck -= amount;
                break;
        }
    }

    public override void Remove(Base_MobStatus target, int amount)
    {
        switch (targetType)
        {
            case Type.atk:
                target.enhancementRate_Power += amount;
                break;

            case Type.def:
                target.enhancementRate_Defence += amount;
                break;

            case Type.spd:
                target.enhancementRate_MoveSpeed += amount;
                break;

            case Type.eye:
                if (target is PlayerStatus player) player.enhancementRate_EyeSight += amount;
                break;

            case Type.luk:
                if (target is PlayerStatus _player) _player.enhancementRate_Luck += amount;
                break;
        }
    }
}
