using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/TreasureLogic/ThrowCount")]
public class TL_ThrowCount : Base_TreasureLogic
{
    // 所持している間、N本ごとに投げるナイフにアビリティーを追加

    [SerializeField] KnifeAbility addedKnifeAbility;
    [field: SerializeField] public int countCycle { get; private set; } = 4;

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        int count = 0;

        status.attack.onThrowKnife.Subscribe(_throw =>
        {
            count++;

            if (count >= countCycle)
            {
                count = 0;

                var ablty = new KnifeAbility(UnityEngine.Object.Instantiate(addedKnifeAbility.abilityLogic),
                                             addedKnifeAbility.effectID);

                _throw.abilities.Add(ablty);

                // 発動を通知
                subject_OnAct.OnNext(Unit.Default);
            }

        }).AddTo(disposables);
    }

    void Hvwbveows(KnifeData_RunTime _knifeData)
    {

    }
}
