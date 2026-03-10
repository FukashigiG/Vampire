using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/TreasureLogic/DamageToHeal")]
public class TL_DamageToHeal : Base_TreasureLogic
{
    // 所持している間、プレイヤーが攻撃を受けるとバフが入る

    [SerializeField] float coolTime;

    [SerializeField] int amount_Heal;

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        bool act = true;
        var token = status.GetCancellationTokenOnDestroy();

        status.onDamaged.Subscribe(async x =>
        {
            if(! act) return;

            act = false;

            status.HealHP(amount_Heal);

            subject_OnAct.OnNext(Unit.Default);

            // 待つ
            try
            {
                await UniTask.Delay((int)(coolTime * 1000), cancellationToken: token);
            }
            catch (System.OperationCanceledException)
            {
                return;
            }

            act = true;

        }).AddTo(disposables);
    }
}
