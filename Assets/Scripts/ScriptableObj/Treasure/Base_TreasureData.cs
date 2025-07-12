using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/Treasure Data/zzzDont")]
public abstract class Base_TreasureData : ScriptableObject
{
    public string _name;
    [TextArea] public string _description;
    public Sprite icon;

    // ゲットした時の処理
    public abstract void OnAdd(PlayerStatus status);

    // 削除された時の処理
    public abstract void OnRemove(PlayerStatus status);

    // 特定のアクションに反応する処理
    public abstract void SubscribeToEvent();
}
