using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/TreasureLogic/ReferHP_BoostElementPoint")]
public class TL_ReferHP_BoostElementPoint : Base_TreasureLogic
{
    // N属性のナイフの属性値をHPに比例反比例して上昇させる

    [SerializeField] Element targetElement;

    [SerializeField] int maxBonusValue;

    [SerializeField] bool reverse;

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        status.attack.onThrowKnife.Subscribe(_knife =>
        {
            if(_knife.element != targetElement) return;

            if (!reverse)
            {
                _knife.elementPower += (int)((float)maxBonusValue * ((float)status.hitPoint.Value / (float)status.maxHP));
            }
            else
            {
                _knife.elementPower += (int)((float)maxBonusValue * (1f - (float)status.hitPoint.Value / (float)status.maxHP));
            }

        }).AddTo(disposables);
    }
}
