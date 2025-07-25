using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyKnifeCtrler : Base_KnifeCtrler
{
    [SerializeField] LayerMask targetLayer;

    [SerializeField] float radius;

    protected override void Start()
    {
        base.Start();

        //一定範囲内の敵を配列に格納
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, targetLayer);

        // リストにある敵それぞれに対して
        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent(out IDamagable i_d))
            {
                // powerの半分のダメージを与える
                i_d.TakeDamage((int)(power / 2), transform.position);
            }
        }
    }
}
