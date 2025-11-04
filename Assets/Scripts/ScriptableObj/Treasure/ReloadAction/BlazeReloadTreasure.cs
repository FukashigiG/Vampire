using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/Treasure Data/Reload/BlazeReload")]
public class BlazeReloadTreasure : Base_ReloadActionTreasure
{
    // リロード時、周囲の敵にデバフ

    [SerializeField] StatusEffectType effectType;
    [SerializeField] string effectID;
    [SerializeField] float duration;
    [SerializeField] int amount;
    [SerializeField] float radius;
    [SerializeField] LayerMask targetLayer;
    [SerializeField] GameObject effect;

    public override void ReloadAction(PlayerStatus status, List<KnifeData_RunTime> knives)
    {
        // 中心
        Vector2 posi = status.transform.position;

        Collider2D[] hits = Physics2D.OverlapCircleAll(posi, radius, targetLayer);

        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent(out Base_MobStatus ms))
            {
                ms.ApplyStatusEffect(effectType, effectID, duration);

                if(effect != null) Instantiate(effect, hit.transform.position, Quaternion.identity);
            }
        }
    }
}
