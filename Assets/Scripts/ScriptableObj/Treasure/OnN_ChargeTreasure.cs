using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/Treasure Data/OnN_Charge")]
public class OnN_ChargeTreasure : Base_TreasureData
{
    // 1サイクル内に投げるN本目以降のナイフを教化

    [field: SerializeField] public int border { get; private set; } = 8;

    int count = 0;

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
            count++;

            if (count >= border)
            {
                Hvwbveows(_knifeData);
            }

        }).AddTo(disposables);

        // リロード時にカウントをリセット
        status.attack.onReload.Subscribe(_ =>
        {
            count = 0;

        }).AddTo(disposables);
    }

    void Hvwbveows(KnifeData_RunTime _knifeData)
    {

    }
}
