using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/Treasure Data/BoostKnifeAbilityProbability")]
public class BoostKnifeAbilityProbabilityTreasure : Base_TreasureData
{
    // 所持している間、プレイヤーの扱うナイフの特定の特殊能力の発動確率を上昇

    // 強化させたい特殊能力
    [field: SerializeField] public Base_KnifeAbilityLogic targetAbilityLogic { get; private set; }
    // ↑の型を示す
    System.Type targetType => targetAbilityLogic.GetType();

    [SerializeField] int amount_Boost_Percentage;

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
            // 対象のアビリティーロジックがあれば、それを取得
            KnifeAbility matchedAbility = _throw.abilities
                .FirstOrDefault(effect => effect.abilityLogic.GetType() == targetType);

            if (matchedAbility != null)
            {
                // 発生確率を上げる
                matchedAbility.abilityLogic.probability_Percent += amount_Boost_Percentage;

                subject_OnAct.OnNext(this);
            }
        })
        .AddTo(disposables);
    }
}
