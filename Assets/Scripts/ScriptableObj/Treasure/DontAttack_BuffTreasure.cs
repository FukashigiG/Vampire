using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System.Linq;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/Treasure Data/DontAttack_Buff")]
public class DontAttack_BuffTreasure : Base_TreasureData
{
    // 所持している間、特定の状態変化効果を受けた敵の死亡に反応して、その敵の周囲に、死んだ敵の最大HPの半分でダメージ

    [SerializeField] StatusEffectType targetEffectType;
    [SerializeField] GameObject effect;
    [SerializeField] float radius;
    [SerializeField] LayerMask targetLayer;
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
        bool standBy = false;

        status.onDamaged.Subscribe(async x =>
        {


            try
            {
                // 待つ
                await UniTask.Delay((int)(coolDownSeconds * 1000), cancellationToken: token);

                // スタンバイ状態に以降
                standBy = true;
            }
            catch (System.OperationCanceledException)
            {
                return;
            }

        }).AddTo(disposables);

        EnemyStatus.onDie.Subscribe(async x =>
        {
            // クールタイム中か、敵が目的の状態異常でないなら無視
            if (isCooling || !  x.status.IsStatusEffectTypeActive(targetEffectType)) return;

            isCooling = true;

            // 死んだ敵の位置を中心地とする
            Vector2 center = x.status.transform.position;

            // 周囲の敵を一括で取得
            Collider2D[] hits = Physics2D.OverlapCircleAll(center, radius, targetLayer);

            // それぞれに対して
            foreach (Collider2D hit in hits)
            {
                if (hit.TryGetComponent(out Base_MobStatus ms))
                {
                    // 当たった本人には追加ダメージは発生しない
                    if (ms != status) ms.GetAttack((x.status.maxHP / 2), center);
                }
            }

            // エフェクト生成
            Instantiate(effect, center, Quaternion.identity);

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
