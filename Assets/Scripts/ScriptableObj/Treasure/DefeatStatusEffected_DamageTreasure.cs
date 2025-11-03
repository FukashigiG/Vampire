using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/Treasure Data/DefeatStatusEffected_Damage")]
public class DefeatStatusEffected_DamageTreasure : Base_TreasureData
{
    // 所持している間、特定の状態変化効果を受けた敵の死亡に反応して、その敵の周囲に、死んだ敵の最大HPの半分でダメージ

    [SerializeField] StatusEffectType targetEffectType;
    [SerializeField] GameObject effect;
    [SerializeField] float radius;
    [SerializeField] LayerMask targetLayer;

    public override void OnAdd(PlayerStatus status)
    {

    }

    public override void OnRemove(PlayerStatus status)
    {

    }

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        EnemyStatus.onDie.Subscribe(x =>
        {
            if (x.status.IsStatusEffectTypeActive(targetEffectType))
            {
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

                Instantiate(effect, center, Quaternion.identity);
            }

        }).AddTo(disposables);
    }
}
