using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[CreateAssetMenu(fileName = "NewKnifeAbility", menuName = "Game Data/KnifeAbility/Electric")]
public class KnifeAbility_Electric : Base_KnifeAbility
{
    public float magnification;
    public float radius;
    public LayerMask targetLayer;
    [SerializeField] GameObject effect;

    protected override void ActivateEffect_OnHit(Base_MobStatus status, Vector2 posi, KnifeData_RunTime knifeData)
    {
        // 周囲の敵に小ダメージ

        Collider2D[] hits = Physics2D.OverlapCircleAll(posi, radius, targetLayer);

        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent(out Base_MobStatus ms))
            {
                // 当たった本人には追加ダメージは発生しない
                if (ms != status) ms.GetAttack((int)(knifeData.power * magnification), posi);

                Instantiate(effect, hit.transform.position, Quaternion.identity);
            }
        }
    }
}
