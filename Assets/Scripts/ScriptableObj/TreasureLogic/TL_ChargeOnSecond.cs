using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/TreasureLogic/ChargeOnSecond")]
public class TL_ChargeOnSecond : Base_TreasureLogic
{
    // HPがN割以上なら、毎秒必殺技ゲージをNチャージする

    [SerializeField] int chargeAmount;
    [SerializeField] int border_HP_Percent;

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        status.onSecond.Subscribe(x =>
        {
            float ratio_HP = (float)status.hitPoint.Value / (float)status.maxHP * 100f;

            if (ratio_HP < border_HP_Percent) return;

            status.attack.AbilityCharge(chargeAmount); 

        }).AddTo(disposables);
    }
}
