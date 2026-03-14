using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/TreasureLogic/SummonObj/Heal")]
public class TL_SummonObj_Heal : Base_TreasureLogic
{
    // 所持している間、ナイフを捨てると何かしらを召喚

    // 精霊のプレハブ
    [SerializeField] GameObject summonObject;

    [SerializeField] int requiredCount = 3;

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        int count = 0;

        status.onHeal.Subscribe(x =>
        {
            count++;

            if(count >= requiredCount)
            {
                Instantiate(summonObject, (Vector2)status.transform.position + Random.insideUnitCircle * 3f, Quaternion.identity);

                count = 0;
            }

        }).AddTo(disposables);
    }
}
