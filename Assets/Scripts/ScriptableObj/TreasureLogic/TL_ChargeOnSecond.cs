using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/TreasureLogic/ChargeOnSecond")]
public class TL_ChargeOnSecond : Base_TreasureLogic
{
    // –ˆ•b•KŽE‹ZƒQ[ƒW‚ðNƒ`ƒƒ[ƒW‚·‚é

    [SerializeField] int chargeAmount;

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        status.onSecond.Subscribe(x =>
        {
            status.attack.AbilityCharge(chargeAmount);

        }).AddTo(disposables);
    }
}
