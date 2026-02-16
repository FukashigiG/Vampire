using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "NewBossEnemyAct", menuName = "Game Data/BossEnemyAct/BulletStorm")]
public class BEA_BulletStorm : Base_BossEnemyAct
{
    // ベーシックな弾発射処理

    [SerializeField] GameObject prefab_Bullet;
    [SerializeField] int num_Bullet;
    [SerializeField] float damageMultiple = 0.5f;

    [SerializeField] float range_X;
    [SerializeField] float range_Y;

    public async override UniTask Action(EnemyCtrler_BigBoss ctrler, CancellationToken token)
    {
        // プレイヤーの方向を取得
        Vector2 dir = (ctrler.target.position - ctrler.transform.position).normalized;

        GameObject bullet = null;
        Quaternion targetRotation = Quaternion.FromToRotation(Vector2.up, dir);

        ctrler._animator.SetTrigger("Spell");

        for (int i = 0; i < num_Bullet; i++)
        {
            var bulletPoint = GetBulletPoint(ctrler.transform.position, targetRotation);

            bullet = Instantiate(prefab_Bullet, bulletPoint, targetRotation);

            bullet.GetComponent<EP_Bullet>().Initialize((int)(ctrler._enemyStatus.power * damageMultiple), 0);

            await UniTask.Delay((int)(75), cancellationToken: token);
        }

        await UniTask.Delay(1000, cancellationToken: token);
    }

    Vector2 GetBulletPoint(Vector2 centerPoint, Quaternion rotate)
    {
        float randomX = Random.Range(-range_X / 2, range_X / 2);
        float randomY = Random.Range(-range_Y / 2, range_Y / 2);

        // 原点を中心としたランダムな四角範囲内の点を取得
        Vector2 basePoint = new Vector2(randomX, randomY);

        // ボスの座標、目的方向と合成
        Vector2 worldPoint = centerPoint + (Vector2)(rotate * basePoint);

        return worldPoint;
    }
}
