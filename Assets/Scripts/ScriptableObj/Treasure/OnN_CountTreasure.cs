using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/Treasure Data/OnN_Count")]
public class OnN_CountTreasure : Base_TreasureData
{
    // 所持している間、N本ごとに投げるナイフにHSpEを追加

    [SerializeField] KnifeAbility addedKnifeAbility;
    [field: SerializeField] public int countCycle { get; private set; } = 4;

    public override void OnAdd(PlayerStatus status)
    {

    }

    public override void OnRemove(PlayerStatus status)
    {

    }

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        int count = 0;

        status.attack.onThrowKnife.Subscribe(_knifeData =>
        {
            count++;

            if (count >= countCycle)
            {
                count = 0;

                _knifeData.abilities.Add(addedKnifeAbility);
            }

        }).AddTo(disposables);
    }

    void Hvwbveows(KnifeData_RunTime _knifeData)
    {

    }
}
