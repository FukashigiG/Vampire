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
        // 得意属性を追加
        status.masteredElements.Add(addedElement);
    }

    public override void OnRemove(PlayerStatus status)
    {
        // リストの中に追加した属性があれば、それを削除
        if(status.masteredElements.Contains(addedElement)) status.masteredElements.Remove(addedElement);
    }

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {

    }
}
