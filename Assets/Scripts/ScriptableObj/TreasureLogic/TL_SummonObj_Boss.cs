using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/TreasureLogic/SummonObj/Boss")]
public class TL_SummonObj_Boss : Base_TreasureLogic
{
    // 所持している間、ボス出現時に精霊を複数体召喚

    // 精霊のプレハブ
    [SerializeField] GameObject summonObject;

    // 召喚（生成）数
    [SerializeField] int num_Summon;

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        GameAdmin.Instance.onBossAppear.Subscribe(x =>
        {
            for (int i = 0; i < num_Summon; i++)
            {
                Instantiate(summonObject, status.transform.position, Quaternion.identity);
            }

        }).AddTo(disposables);
    }
}
