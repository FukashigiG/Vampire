using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/Treasure Data/OnN_ElementCount")]
public class OnN_ElementCountTreasure : Base_TreasureData
{
    // 所持している間、特定属性のナイフをN本投げるごとに自分のステータスが上昇する
    // 

    [SerializeField] Element targetElement;
    [SerializeField] int border_Count;
    [SerializeField] int bonusAmount_Each;
    [SerializeField] int bonusLimit;

    int cullentBouns;

    public override void OnAdd(PlayerStatus status)
    {
        cullentBouns = 0;
    }

    public override void OnRemove(PlayerStatus status)
    {

    }

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        int count = 0;

        // 特定の属性を投げるたびカウントをプラス
        status.attack.onThrowKnife.Subscribe(_throw =>
        {
            if (_throw.knifeData.element == targetElement)
            {
                count++;

                if (count >= border_Count)
                {
                    count = 0;

                    cullentBouns++;

                    if(cullentBouns >= bonusLimit) cullentBouns = bonusLimit;

                    _throw.knifeData.power += cullentBouns;
                }
            }

        }).AddTo(disposables);
    }

    void Hvwbveows(KnifeData_RunTime _knifeData)
    {

    }
}
