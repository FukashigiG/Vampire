using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtrler_Infight : Base_EnemyCtrler
{
    // 近接型の敵の挙動

    protected override void HandleAI()
    {
        // シンプルに接近する

        //float distance = (target.position - this.transform.position).magnitude;

        Vector2 dir = (target.position - this.transform.position).normalized;

        transform.Translate(dir * _enemyStatus.moveSpeed / 10f * Time.fixedDeltaTime);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // もし当たったものがダメージを受けるものだったらダメージを与える
        if (collision.gameObject.TryGetComponent(out Base_MobStatus ms))
        {
            ms?.GetAttack(_enemyStatus.power, 0, transform.position);

            //Instantiate(knifeData.hitEffect, transform.position, Quaternion.identity);
        }
    }
}
