using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/TreasureLogic/ExtraDuplicate")]
public class TL_ExtraDuplicate : Base_TreasureLogic
{
    // 重複度N以下のナイフの重複度をF上げる

    [SerializeField] int border;
    [SerializeField] int incrementAmount;

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        status.attack.onThrowKnife.Subscribe(_throw =>
        {
            if (_throw.count_Multiple <= border) _throw.count_Multiple += incrementAmount;
        })
        .AddTo(disposables);
    }
}
