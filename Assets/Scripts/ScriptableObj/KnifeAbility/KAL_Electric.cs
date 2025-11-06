using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[CreateAssetMenu(fileName = "NewKnifeAbility", menuName = "Game Data/KnifeAbility/Electric")]
public class KAL_Electric : Base_KnifeAbilityLogic
{
    [SerializeField] float radius;
    [SerializeField] LayerMask targetLayer;
    [SerializeField] GameObject effect;

    public override void ActivateAbility(Base_MobStatus status, GameObject knifeObj, KnifeData_RunTime knifeData, float modifire, string effectID)
    {
        base.ActivateAbility(status, knifeObj, knifeData, modifire, effectID);

        Vector2 center = knifeObj.transform.position;

        // 周囲の敵に属性値の分ダメージ

        Collider2D[] hits = Physics2D.OverlapCircleAll(center, radius * modifire, targetLayer);

        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent(out Base_MobStatus ms))
            {
                // 当たった本人には追加ダメージは発生しない
                if (ms != status) ms.GetAttack(0, (int)(knifeData.elementPower * modifire), center);

                Instantiate(effect, hit.transform.position, Quaternion.identity);
            }
        }
    }
}
