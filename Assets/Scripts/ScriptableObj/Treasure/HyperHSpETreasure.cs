using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/Treasure Data/HyperHSpE")]
public class HyperHSpETreasure : Base_TreasureData
{
    // 所持している間、プレイヤーの扱うナイフの特定の特殊能力を強化

    [field: SerializeField] public BaseHSpE hspe { get; private set; }

    System.Type targetType => hspe.GetType();

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

            }
        })
        .AddTo(disposables);
    }
}
