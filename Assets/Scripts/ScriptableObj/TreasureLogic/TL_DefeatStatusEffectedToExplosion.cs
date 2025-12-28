using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/TreasureLogic/DefeatStatusEffected_Damage")]
public class TL_DefeatStatusEffectedToExplosion : Base_TreasureLogic
{
    // 所持している間、特定の状態変化効果を受けた敵の死亡に反応して、その敵の周囲に、死んだ敵の最大HPの半分でダメージ

    [SerializeField] Base_StatusEffectData statusEffect;
    [SerializeField] GameObject effect;
    [SerializeField] float radius;
    [SerializeField] LayerMask targetLayer;
    [SerializeField] float coolDownSeconds;

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        //bool isCooling = false;
        var token = status.GetCancellationTokenOnDestroy();

        /*
        EnemyStatus.onDie.Subscribe(async x =>
        {
            // クールタイム中か、敵が目的の状態異常でないなら無視
            if (isCooling || !  x.status.IsStatusEffectTypeActive(statusEffect)) return;

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
                    // 攻撃値、属性値それぞれ死んだ敵の最大HP÷4でダメージ
                    // 当たった本人には追加ダメージは発生しない
                    if (ms != status) ms.GetAttack(x.status.maxHP / 4, x.status.maxHP / 4, center);
                }
            }

            // エフェクト生成
            Instantiate(effect, center, Quaternion.identity);

            // 発動を通知
            subject_OnAct.OnNext(Unit.Default);

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
        */
    }
}
