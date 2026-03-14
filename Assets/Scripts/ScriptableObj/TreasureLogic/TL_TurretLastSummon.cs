using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System.Linq;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/TreasureLogic/TurretLastSummon")]
public class TL_TurretLastSummon : Base_TreasureLogic
{
    // 所持している間、タレットの消滅時その場に何かしらアイテムを残す

    [SerializeField] GameObject summonedObj;

    [SerializeField] int percent = 20;

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        TurretCtrler.onDestroy.Subscribe(turret =>
        {
            if (Random.Range(1, 101) <= percent) Instantiate(summonedObj, turret.transform.position, Quaternion.identity);

        }).AddTo(disposables);
    }
}
