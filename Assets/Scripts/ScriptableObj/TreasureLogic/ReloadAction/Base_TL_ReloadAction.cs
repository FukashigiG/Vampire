using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class Base_TL_ReloadAction : Base_TreasureLogic
{
    // 所持している間、プレイヤーのリロードに反応して

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        status.attack.onReload.Subscribe(_knives =>
        {
            ReloadAction(status, _knives);
        })
        .AddTo(disposables);
    }

    public virtual void ReloadAction(PlayerStatus status, ReactiveCollection<KnifeData_RunTime> knives)
    {
        // 発動を通知
        subject_OnAct.OnNext(Unit.Default);
    }
}
