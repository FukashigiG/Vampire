using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System.Linq;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/Treasure Data/DiscardKnifeToHeal")]
public class DiscardKnifeToHealTreasure : Base_TreasureData
{
    // 所持している間、数秒に一度、ダメージを受けるとランダムなナイフを消費して回復

    [SerializeField] int healAmount_Percent;
    [SerializeField] float coolDownSeconds;

    public override void OnAdd(PlayerStatus status)
    {

    }

    public override void OnRemove(PlayerStatus status)
    {

    }

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        bool isCooling = false;
        var token = status.GetCancellationTokenOnDestroy();

        status.onDamaged.Subscribe(async x =>
        {
            // クールタイム中か、敵が目的の状態異常でないなら無視
            if (isCooling || status.hitPoint >= 0) return;

            // リストの何番目のナイフを捨てるかを決定
            int random = Random.Range(0, status.inventory.runtimeKnives.Count);

            // 削除関数を実行、結果の可否を保存
            bool y = status.inventory.RemoveKnife(status.inventory.runtimeKnives[random]);

            // 削除が行われなかったらreturn
            if (!y) return;

            // 回復
            status.HealHP(status.maxHP * healAmount_Percent / 100);

            // クールタイムに移行
            isCooling = true;

            // 待つ
            try
            {
                await UniTask.Delay((int)(coolDownSeconds * 1000), cancellationToken: token);
            }
            catch (System.OperationCanceledException)
            {
                return;
            }

            // クールタイム解除
            isCooling = false ;

        }).AddTo(disposables);
    }
}
