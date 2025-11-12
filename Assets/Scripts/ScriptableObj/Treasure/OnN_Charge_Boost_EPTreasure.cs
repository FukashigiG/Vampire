using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/Treasure Data/OnN_Charge_Boost_EP")]
public class OnN_Charge_Boost_EP_Treasure : Base_TreasureData
{
    // 1サイクル内に投げるN本目以降のナイフの属性値を上昇

    [SerializeField] int amount_Plus;

    [field: SerializeField] public int border { get; private set; } = 8;

    

    public override void OnAdd(PlayerStatus status)
    {

    }

    public override void OnRemove(PlayerStatus status)
    {

    }

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        int count = 0;

        status.attack.onThrowKnife.Subscribe(_throw =>
        {
            count++;

            if (count >= border)
            {
                _throw.knifeData.elementPower += amount_Plus;

                // 発動を通知
                subject_OnAct.OnNext(this);
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
