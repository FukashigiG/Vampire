using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

//[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/Treasure Data/zzzDont")]
public abstract class Base_TreasureData : Base_PlayerItem
{

    protected static Subject<Base_TreasureData> subject_OnAct = new Subject<Base_TreasureData>();
    public static IObservable<Base_TreasureData> onAct => subject_OnAct;

    // ゲットした時の処理
    public abstract void OnAdd(PlayerStatus status);

    // 削除された時の処理
    public abstract void OnRemove(PlayerStatus status);

    // 特定のアクションに反応する処理
    public abstract void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables);
}
