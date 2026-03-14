using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System.Linq;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/TreasureLogic/ExTurretBullet")]
public class TL_ExTurretBullet : Base_TreasureLogic
{
    // 所持している間、タレットの弾数が増加

    [SerializeField] int num_ExBullet = 5;

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        TurretCtrler.onAwake.Subscribe(x =>
        {
            x.bulletNum += num_ExBullet;

        }).AddTo(disposables);
    }
}
