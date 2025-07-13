using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/Treasure Data/AddBlaze")]
public class AddBlazeTreasure : Base_TreasureData
{
    // 所持している間、プレイヤーの扱う火属性のナイフ全てに特殊能力”火炎”を付与

    public BaseHSpE blazeData;

    public override void OnAdd(PlayerStatus status)
    {

    }

    public override void OnRemove(PlayerStatus status)
    {

    }

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        status.attack.onThrowKnife.Subscribe(_knifeData =>
        {
            if (_knifeData.element != KnifeData.ElementEnum.red) return;
            if (_knifeData.specialEffects.Contains(blazeData)) return;

            _knifeData.specialEffects.Add(blazeData);

            Debug.Log("Set blaze for : " + _knifeData._name);
        })
        .AddTo(disposables);
    }
}
