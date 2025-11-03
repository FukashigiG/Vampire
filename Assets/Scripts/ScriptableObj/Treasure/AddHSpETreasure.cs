using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/Treasure Data/AddHSpE")]
public class AddHSpETreasure : Base_TreasureData
{
    // 所持している間、プレイヤーの扱う特定の属性のナイフに特殊能力を追加

    [field: SerializeField] public Element targetEnum {  get; private set; }

    [field: SerializeField] public BaseHSpE hspe {  get; private set; }

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
            // そのナイフデータの属性が対象でない、または既にこの能力を持ってるなら無視
            if (_knifeData.element != targetEnum) return;
            if (_knifeData.specialEffects.Contains(hspe)) return;

            // 引数で渡されたナイフのデータに、特定の特殊能力を追加
            _knifeData.specialEffects.Add(Instantiate(hspe));
        })
        .AddTo(disposables);
    }
}
