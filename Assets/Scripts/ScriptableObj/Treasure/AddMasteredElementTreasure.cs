using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/Treasure Data/AddMasteredElement")]
public class AddMasteredElementTreasure : Base_TreasureData
{
    // 所持している間、プレイヤーの得意属性を追加

    [field: SerializeField] public Element addedElement {  get; private set; }

    public override void OnAdd(PlayerStatus status)
    {
        
    }

    public override void OnRemove(PlayerStatus status)
    {

    }

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {

    }
}
