using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/Treasure Data/CriticalBonus")]
public class CriticalBonusTreasure : Base_TreasureData
{
    // 所持している間、クリティカルを食らった敵に追加効果を付与

    [SerializeField] Base_StatusEffectData statusEffect;
    [SerializeField] string effectID;
    [SerializeField] float duration;
    [SerializeField] int amount_Debuff;

    public override void OnAdd(PlayerStatus status)
    {

    }

    public override void OnRemove(PlayerStatus status)
    {

    }

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        // クリティカル発動の発動を購読
        KAL_Critical.onEffectActived.Subscribe(targetStatus  =>
        {
            targetStatus.ApplyStatusEffect(statusEffect, effectID, duration ,amount_Debuff);

        }).AddTo(disposables);
    }
}
