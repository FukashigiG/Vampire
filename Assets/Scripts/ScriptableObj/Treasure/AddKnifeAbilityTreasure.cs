using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/Treasure Data/AddHSpE")]
public class AddKnifeAbilityTreasure : Base_TreasureData
{
    // 所持している間、プレイヤーの扱う特定の属性のナイフに特殊能力を追加

    [field: SerializeField] public Element targetEnum {  get; private set; }

    [field: SerializeField] public KnifeAbility ability {  get; private set; }

    public override void OnAdd(PlayerStatus status)
    {

    }

    public override void OnRemove(PlayerStatus status)
    {

    }

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        status.attack.onThrowKnife.Subscribe(_throw =>
        {
            // そのナイフデータの属性が対象でない、または既にこの能力を持ってるなら無視
            if (_throw.knifeData.element != targetEnum) return;
            // 対象のアビリティーロジックがあれば、それを取得
            KnifeAbility matchedAbility = _throw.knifeData.abilities
                .FirstOrDefault(effect => effect.abilityLogic.GetType() == ability.abilityLogic.GetType());

            // 引数で渡されたナイフのデータに、特定の特殊能力を追加
            _throw.knifeData.abilities.Add(ability);
        })
        .AddTo(disposables);
    }
}
