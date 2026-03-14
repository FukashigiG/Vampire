using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewKnifeAbility", menuName = "Game Data/KnifeAbility/FixTurret")]
public class KAL_FixTurret : Base_KnifeAbilityLogic
{
    // 発動時、周囲のタレットの弾数を回復

    [SerializeField] int num_ExBullet = 5;
    [SerializeField] float radius;
    [SerializeField] LayerMask targetLayer;
    [SerializeField] GameObject effect;

    public override void ActivateAbility(Base_MobStatus status, GameObject knifeObject, KnifeData_RunTime knifeData, string effectID)
    {
        base.ActivateAbility(status, knifeObject, knifeData,effectID);

        Vector2 center = knifeObject.transform.position;

        Collider2D[] hits = Physics2D.OverlapCircleAll(center, radius, targetLayer);

        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent(out TurretCtrler ctrler))
            {
                ctrler.bulletNum += num_ExBullet;

                Instantiate(effect, hit.transform.position, Quaternion.identity);
            }
        }
    }
}
