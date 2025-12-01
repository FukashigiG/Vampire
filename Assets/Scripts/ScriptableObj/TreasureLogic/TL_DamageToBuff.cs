using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/TreasureLogic/DamageToBuff")]
public class TL_DamageToBuff : Base_TreasureLogic
{
    // 所持している間、プレイヤーが攻撃を受けるとバフが入る

    [SerializeField] Base_StatusEffectData statusEffect;
    [SerializeField] string effectID;
    [SerializeField] float duration;
    [SerializeField] int amount;

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        status.onDamaged.Subscribe(x =>
        {
            status.ApplyStatusEffect(statusEffect, effectID, duration, amount);

            subject_OnAct.OnNext(Unit.Default);

        }).AddTo(disposables);
    }
}
