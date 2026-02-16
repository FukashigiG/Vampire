using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System.Linq;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/TreasureLogic/SpiritLast/Heal")]
public class TL_SpiritLastHeal : Base_TreasureLogic
{
    // 所持している間、精霊が寿命を迎えた際、プレイヤーが回復

    [SerializeField] float healAmountMultiple = 2f;

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        SpiritCtrler.onDestroy.Subscribe(spirit =>
        {
            PlayerController.Instance._status.HealHP((int)(spirit.power * healAmountMultiple));

        }).AddTo(disposables);
    }
}
