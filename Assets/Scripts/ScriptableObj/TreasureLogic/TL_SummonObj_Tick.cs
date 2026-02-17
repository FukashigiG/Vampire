using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/TreasureLogic/SummonObj/Tick")]
public class TL_SummonObj_Tick : Base_TreasureLogic
{
    // 所持している間、数秒おきにオブジェクトを生成する

    // 生成対象のプレハブ
    [SerializeField] GameObject summonObject;

    // 効果発動確率
    [SerializeField] int interval_Sec;

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        int count = 0;

        status.onSecond.Subscribe(x =>
        {
            count++;

            if (count >= interval_Sec)
            {
                count = 0;

                // オブジェクト生成
                Instantiate(summonObject, status.transform.position, Quaternion.identity);

                subject_OnAct.OnNext(Unit.Default);
            }

        }).AddTo(disposables);
    }
}
