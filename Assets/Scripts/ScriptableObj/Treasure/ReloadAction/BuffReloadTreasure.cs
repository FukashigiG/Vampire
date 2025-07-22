using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/Treasure Data/Reload/BuffReload")]
public class BuffReloadTreasure : Base_ReloadActionTreasure
{
    public StatusEffectType statusEffectType;

    public float duration;
    public float amount;

    public override void ReloadAction(PlayerStatus status, List<KnifeData> knives)
    {
        status.ApplyStatusEffect(statusEffectType, duration, amount);
    }
}
