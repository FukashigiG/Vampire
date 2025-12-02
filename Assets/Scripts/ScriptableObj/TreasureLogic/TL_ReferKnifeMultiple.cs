using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/TreasureLogic/ReferKnifeMultiple")]
public class TL_ReferKnifeMultiple : Base_TreasureLogic
{
    // 重複度N以上のナイフの基礎攻撃力を上げる

    [SerializeField] int border;
    [SerializeField] int enhancementValue;

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        status.attack.onThrowKnife.Subscribe(_throw =>
        {
            if(_throw.count_Multiple >= border)
            {
                _throw.power += enhancementValue;
            }
        })
        .AddTo(disposables);
    }
}
