using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/Treasure Data/FreezeBonus")]
public class FreezeBonusTreasure : Base_TreasureData
{
    // 所持している間、特定の状態異常の敵が死ぬと、周囲にいた他の敵を状態異常にする
    // 例：凍結になった敵の周囲の敵の素早さを下げる

    [Header("反応対象")]
    [SerializeField] Base_StatusEffectData targetStatusEffect;

    [Header("敵感知関連")]
    [SerializeField] float radius;
    [SerializeField] LayerMask targetLayer;

    [Header("与デバフ関連")]
    [SerializeField] Base_StatusEffectData statusEffect;
    [SerializeField] string effectID;
    [SerializeField] float duration;
    [SerializeField] int amount_Debuff;

    [Header("その他")]
    [SerializeField] float coolDownSeconds;
    [SerializeField] GameObject fx;

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

        EnemyStatus.onGetStatusEffect.Subscribe(async effect =>
        {
            // 目的の状態でない, クールタイム中なら無視
            if (effect.effect != targetStatusEffect || isCooling) return;

            isCooling = true;

            // 中心位置の取得
            Vector2 center = effect.status.transform.position;

            // 周囲の敵を取得
            Collider2D[] hits = Physics2D.OverlapCircleAll(center, radius, targetLayer);

            // それらに対し
            foreach (Collider2D hit in hits)
            {
                if (hit.TryGetComponent(out EnemyStatus enemy))
                {
                    // 当たった本人には発生しない
                    if (enemy != status) enemy.ApplyStatusEffect(statusEffect, effectID, duration, amount_Debuff);
                }
            }

            Instantiate(fx, center, Quaternion.identity);

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
            isCooling = false;


        }).AddTo(disposables);
    }
}
