using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/Treasure Data/Reload/BuffReload")]
public class BuffReloadTreasure : Base_ReloadActionTreasure
{
    public StatusEffectType statusEffectType;

    public float duration;
    public int amount_percent;
    [SerializeField] string effectID;

    public override void ReloadAction(PlayerStatus status, List<KnifeData_RunTime> knives)
    {
        status.ApplyStatusEffect(statusEffectType, effectID, duration, amount_percent);
    }
}
