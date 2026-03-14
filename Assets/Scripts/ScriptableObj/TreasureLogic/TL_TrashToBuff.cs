using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/TreasureLogic/TrashToBuff")]
public class TL_TrashToBuff : Base_TreasureLogic
{
    // 所持している間、ナイフを捨てるとバフ

    [SerializeField] Base_StatusEffectData statusEffect;
    [SerializeField] string effectid;
    [SerializeField] float duration;
    [SerializeField] int amount;

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        status.attack.onTrashKnife.Subscribe(_throw =>
        {
            status.ApplyStatusEffect(statusEffect, effectid, duration, amount);

        }).AddTo(disposables);
    }
}
