using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/TreasureLogic/SummonObj/Trash")]
public class TL_SummonObj_Trash : Base_TreasureLogic
{
    // 所持している間、ナイフを捨てると何かしらを召喚

    // 精霊のプレハブ
    [SerializeField] GameObject summonObject;

    [SerializeField] int actRate_P = 50;

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        status.attack.onTrashKnife.Subscribe(x =>
        {
            if(Random.Range(0, 101) <= actRate_P) Instantiate(summonObject, (Vector2)status.transform.position + Random.insideUnitCircle * 3f, Quaternion.identity);

        }).AddTo(disposables);
    }
}
