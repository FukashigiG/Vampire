using System;
using UniRx;
using UnityEngine;

public abstract class Base_TreasureLogic : ScriptableObject
{
    protected Subject<Unit> subject_OnAct = new Subject<Unit>();
    public IObservable<Unit> onAct => subject_OnAct;

    // ゲットした時の処理
    public virtual void AddedTrigger(PlayerStatus status)
    {

    }

    // 削除された時の処理
    public virtual void RemovedTrigger(PlayerStatus status)
    {

    }

    // 特定のアクションに反応する処理
    public virtual void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        subject_OnAct.OnNext(Unit.Default);
    }
}
