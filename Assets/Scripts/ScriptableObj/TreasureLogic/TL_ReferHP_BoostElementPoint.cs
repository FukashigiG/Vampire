using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/TreasureLogic/ReferHP_BoostElementPoint")]
public class TL_ReferHP_BoostElementPoint : Base_TreasureLogic
{
    // N属性のナイフの属性値を体力の10%分の数値だけ上昇させる

    [SerializeField] Element targetElement;

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        status.attack.onThrowKnife.Subscribe(_knife =>
        {
            if(_knife.element != targetElement) return;

            _knife.elementPower += status.hitPoint.Value / 10;

        }).AddTo(disposables);
    }
}
