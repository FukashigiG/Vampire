using System;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/TreasureData")]
public class TreasureData : Base_PlayerItem
{
    [field: SerializeField] public string description {  get; private set; }
    [field: SerializeField] public Base_TreasureLogic logic {  get; private set; }

    protected static Subject<TreasureData> subject_OnAct = new Subject<TreasureData>();
    public static IObservable<TreasureData> onAct => subject_OnAct;

    // ƒQƒbƒg‚µ‚½‚Ìˆ—
    public void OnAdd(PlayerStatus status, CompositeDisposable disposables)
    {
        logic.AddedTrigger(status);
        logic.SubscribeToEvent(status, disposables);

        logic.onAct.Subscribe(x =>
        {
            subject_OnAct.OnNext(this);

        }).AddTo(disposables);
    }

    // íœ‚³‚ê‚½‚Ìˆ—
    public void OnRemove(PlayerStatus status)
    {
        logic.RemovedTrigger(status);
    }
}
