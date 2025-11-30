using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/TreasureLogic/ThrowCount_Element")]
public class TL_ThrowElementCount : Base_TreasureLogic
{
    // 所持している間、特定属性のナイフをN本投げるごとに自分のステータスが上昇する
    // 

    [SerializeField] Element targetElement;
    [SerializeField] int border_Count;
    [SerializeField] int bonusAmount_Each;
    [SerializeField] int bonusLimit;

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        int cullentBouns = 0;
        int count = 0;

        // 特定の属性を投げるたびカウントをプラス
        status.attack.onThrowKnife.Subscribe(_throw =>
        {
            if (_throw.element == targetElement)
            {
                count++;

                if (count >= border_Count)
                {
                    count = 0;

                    cullentBouns++;

                    if(cullentBouns >= bonusLimit) cullentBouns = bonusLimit;

                    _throw.power += cullentBouns;

                    // 発動を通知
                    subject_OnAct.OnNext(Unit.Default);
                }
            }

        }).AddTo(disposables);
    }

    void Hvwbveows(KnifeData_RunTime _knifeData)
    {

    }
}
