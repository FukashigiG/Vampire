using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/Treasure Data/BoostHSpE")]
public class BoostHSpEProbabilityTreasure : Base_TreasureData
{
    // 所持している間、プレイヤーの扱うナイフの特定の特殊能力の発動確率を上昇

    // 強化させたい特殊能力
    [field: SerializeField] public BaseHSpE targetHspe { get; private set; }
    // ↑の型を示す
    System.Type targetType => targetHspe.GetType();

    [SerializeField] int amount_Boost_Percentage;

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
            // 対象のHSpEがあれば、それを取得
            BaseHSpE matchedEffect = _knifeData.specialEffects
                .FirstOrDefault(effect => effect.GetType() == targetType);

            if (matchedEffect != null)
            {
                // 発生確率をプラスする
                matchedEffect.probability_Percent += amount_Boost_Percentage;
            }
        })
        .AddTo(disposables);
    }
}
