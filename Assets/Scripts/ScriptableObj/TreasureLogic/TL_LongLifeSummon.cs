using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System.Linq;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/TreasureLogic/LongLifeSummon")]
public class TL_LongLifeSummon : Base_TreasureLogic
{
    // 所持している間、召喚物の寿命が伸びる

    [SerializeField] float extLife = 1f;

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        ISummonable.onAwake.Subscribe( x =>
        {
            x.lifeTime += extLife;

        }).AddTo( disposables );
    }
}
