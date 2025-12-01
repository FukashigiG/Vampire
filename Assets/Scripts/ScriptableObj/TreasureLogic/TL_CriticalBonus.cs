using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/TreasureLogic/CriticalBonus")]
public class TL_CriticalBonus : Base_TreasureLogic
{
    // 所持している間、クリティカルを食らった敵に追加効果を付与

    [SerializeField] Base_StatusEffectData statusEffect;
    [SerializeField] string effectID;
    [SerializeField] float duration;
    [SerializeField] int amount_Debuff;

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        // クリティカル発動の発動を購読
        KAL_Critical.onEffectActived.Subscribe(targetStatus  =>
        {
            targetStatus.ApplyStatusEffect(statusEffect, effectID, duration ,amount_Debuff);

            Debug.Log("asa");

            // 発動を通知
            subject_OnAct.OnNext(Unit.Default);

        }).AddTo(disposables);
    }
}
