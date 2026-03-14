using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/TreasureLogic/TrashToHeal")]
public class TL_TrashToHeal : Base_TreasureLogic
{
    // 所持している間、ナイフを捨てると回復

    [field: SerializeField] int healAmount = 2;

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        status.attack.onTrashKnife.Subscribe(_throw =>
        {
            status.HealHP(healAmount);

        }).AddTo(disposables);
    }
}
