using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/Treasure Data/Reload/HealReload")]
public class HealReloadTreasure : Base_ReloadActionTreasure
{
    public float healRatio;

    public override void ReloadAction(PlayerStatus status, ReactiveCollection<KnifeData_RunTime> knives)
    {
        status.HealHP((int)(status.maxHP * healRatio));

        base.ReloadAction(status, knives);
    }
}
