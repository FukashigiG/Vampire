using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/Treasure Data/HyperHSpE")]
public class OnN_CountTreasure : Base_TreasureData
{
    // �������Ă���ԁAN�{���Ƃɓ�����i�C�t������

    [field: SerializeField] public int countCycle { get; private set; } = 4;

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

            if (count >= countCycle)
            {
                count = 0;

                Hvwbveows(_knifeData);
            }

        }).AddTo(disposables);
    }

    void Hvwbveows(KnifeData_RunTime _knifeData)
    {

    }
}
