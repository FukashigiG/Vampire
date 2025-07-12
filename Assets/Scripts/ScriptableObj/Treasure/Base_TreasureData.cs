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

    public abstract void OnAdd(PlayerStatus status);
    public abstract void OnRemove(PlayerStatus status);
    public abstract void SubscribeToEvent();
}
