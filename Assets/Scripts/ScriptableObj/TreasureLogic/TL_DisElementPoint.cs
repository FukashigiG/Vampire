using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/TreasureLogic/DisElementPoint")]
public class TL_DisElementPoint : Base_TreasureLogic
{
    // ナイフの属性値を犠牲に攻撃力を挙げる

    [SerializeField] int cut_EP = 8;
    [SerializeField] int boost_Power = 20;

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        status.attack.onThrowKnife.Subscribe(_knife =>
        {
            _knife.elementPower -= cut_EP;
            if(_knife.elementPower < 0) _knife.elementPower = 0;

            _knife.power += boost_Power;

        }).AddTo(disposables);
    }
}
