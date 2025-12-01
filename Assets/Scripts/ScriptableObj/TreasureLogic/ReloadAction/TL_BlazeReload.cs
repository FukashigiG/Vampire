using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/TreasureLogic/Reload/BlazeReload")]
public class TL_BlazeReload : Base_TL_ReloadAction
{
    // リロード時、N属性のナイフがM本以上含まれていれば、周囲の敵にデバフ

    [Header("発動条件")]
    [SerializeField] Element targetElement;
    [SerializeField] int border;

    [Header("状態効果詳細")]
    [SerializeField] Base_StatusEffectData statusEffect;
    [SerializeField] string effectID;
    [SerializeField] float duration;
    [SerializeField] int amount;

    [Header("敵探索詳細")]
    [SerializeField] float radius;
    [SerializeField] LayerMask targetLayer;
    [SerializeField] GameObject effect;

    public override void ReloadAction(PlayerStatus status, ReactiveCollection<KnifeData_RunTime> knives)
    {
        // リロードした際の、N属性のナイフの数を数える
        int count = knives.Count(x => x.element == targetElement);

        Debug.Log($"{count}本あった");

        // 規定の数に達していないならreturn
        if (count < border) return;

        // 中心
        Vector2 posi = status.transform.position;

        // 周囲の敵を取得
        Collider2D[] hits = Physics2D.OverlapCircleAll(posi, radius, targetLayer);

        // それぞれの敵に対して
        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent(out Base_MobStatus ms))
            {
                ms.ApplyStatusEffect(statusEffect, effectID, duration);
            }
        }

        if (effect != null) Instantiate(effect, status.transform.position, Quaternion.identity);

        base.ReloadAction(status, knives);
    }
}
