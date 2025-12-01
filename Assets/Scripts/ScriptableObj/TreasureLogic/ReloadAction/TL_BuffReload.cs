using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/TreasureLogic/Reload/BuffReload")]
public class TL_BuffReload : Base_TL_ReloadAction
{
    public Base_StatusEffectData statusEffect;

    public float duration;
    public int amount_percent;
    [SerializeField] string effectID;

    public override void ReloadAction(PlayerStatus status, ReactiveCollection<KnifeData_RunTime> knives)
    {
        status.ApplyStatusEffect(statusEffect, effectID, duration, amount_percent);

        base.ReloadAction(status, knives);
    }
}
