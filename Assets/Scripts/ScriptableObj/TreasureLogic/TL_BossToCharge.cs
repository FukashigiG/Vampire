using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System.Linq;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/TreasureLogic/Boss_Charge")]
public class TL_BossToCharge: Base_TreasureLogic
{
    // 所持している間、ボス戦開始時に必殺技超チャージ

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        GameAdmin.Instance.onBossAppear.Subscribe(x =>
        {
            // 一本につき最大HPのN%回復
            // 上限50とする
            status.attack.AbilityCharge(999);

        }).AddTo(disposables);
    }
}
