using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/Treasure Data/OnN_Count")]
public class OnN_CountTreasure : Base_TreasureData
{
    // 所持している間、N本ごとに投げるナイフを教化

    [SerializeField] BaseHSpE addedHSpE;
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

                _knifeData.specialEffects.Add(Instantiate(addedHSpE));
            }

        }).AddTo(disposables);
    }

    void Hvwbveows(KnifeData_RunTime _knifeData)
    {

    }
}
