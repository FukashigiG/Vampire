using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System.Linq;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/TreasureLogic/SpiritBuff")]
public class TL_SpiritBuff : Base_TreasureLogic
{
    // ŠŽ‚µ‚Ä‚¢‚éŠÔAŽõ–½‚ð‹]µ‚É¶¬‚³‚ê‚é¸—ì‚ª‹­‰»‚³‚ê‚é

    [SerializeField] int buffAmount_Power;

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        SpiritCtrler.onAwake.Subscribe(spirit =>
        {
            spirit.LifeTime *= 0.6f;

            spirit.interval_Shot_Sec *= 0.6f;

            spirit.power += buffAmount_Power;

        }).AddTo(disposables);
    }
}
