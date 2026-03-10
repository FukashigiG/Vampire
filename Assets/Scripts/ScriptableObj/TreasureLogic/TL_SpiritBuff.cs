using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System.Linq;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/TreasureLogic/SpiritBuff")]
public class TL_SpiritBuff : Base_TreasureLogic
{
    // 所持している間、寿命を犠牲に生成される精霊が強化される

    [SerializeField] int buffAmount_Power;

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        SpiritCtrler.onAwake.Subscribe(spirit =>
        {
            spirit.lifeTime *= 0.6f;

            spirit.interval_Shot_Sec *= 0.6f;

            spirit.power += buffAmount_Power;

        }).AddTo(disposables);
    }
}
