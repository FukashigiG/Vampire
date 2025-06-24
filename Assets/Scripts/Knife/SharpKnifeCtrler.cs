using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharpKnifeCtrler : Base_KnifeCtrler
{

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        // もし当たったもんがダメージを受けるものだったらダメージ
        if (collision.TryGetComponent(out IDamagable i_d))
        {
            i_d.TakeDamage(power, transform.position);

            // 当たった時このオブジェクトをDestroyする処理を削除したため、貫通するようになってる
        }
    }
}
