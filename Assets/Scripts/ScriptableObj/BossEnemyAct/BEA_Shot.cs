using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "NewBossEnemyAct", menuName = "Game Data/BossEnemyAct/Basic/Shot")]
public class BEA_Shot : Base_BossEnemyAct
{
    // ベーシックな弾発射処理

    [SerializeField] GameObject prefab_Bullet;
    [SerializeField] int num_Bullet;
    [SerializeField] float divergenceAngle;
    [SerializeField] float damageMultiple = 0.5f;

    [Serializable] enum ShotType { oneShot, rapidFire}
    [SerializeField] ShotType shotType;

    public async override UniTask Action(EnemyCtrler_BigBoss ctrler, CancellationToken token)
    {
        // プレイヤーの方向を取得
        Vector2 dir = (ctrler.target.position - ctrler.transform.position).normalized;

        float angleOffset = 0;
        GameObject bullet = null;
        Quaternion baseRotation = Quaternion.FromToRotation(Vector2.up, dir);

        ctrler._animator.SetTrigger("Spell");

        switch (shotType)
        {
            // 単射タイプの挙動
            case ShotType.oneShot:

                for (int i = 0; i < num_Bullet; i++)
                {
                    // 弾の数が２以上なら以下の計算を実行
                    if (num_Bullet > 1)
                    {
                        // 今投げる角度を求める
                        angleOffset = Mathf.Lerp(divergenceAngle * -1 / 2, divergenceAngle / 2, (float)i / (num_Bullet - 1));
                    }

                    // Quaternionに変換
                    Quaternion rotationOffset = Quaternion.Euler(0, 0, angleOffset);

                    // ベースの方向と合成
                    Quaternion finalRotation = baseRotation * rotationOffset;

                    // 弾を生成
                    bullet = Instantiate(prefab_Bullet, ctrler.transform.position, finalRotation);

                    // 弾を初期化
                    bullet.GetComponent<EP_Bullet>().Initialize((int)(ctrler._enemyStatus.power * damageMultiple), 0);
                }

                break;

            // 連射タイプの挙動
            case ShotType.rapidFire:

                Quaternion targetRotation;

                // 中心角度からランダムにずらして弾をばらまく
                for (int i = 0; i < num_Bullet; i++)
                {
                    angleOffset = Random.Range(divergenceAngle * -1 / 2, divergenceAngle / 2);

                    targetRotation = baseRotation * Quaternion.Euler(0, 0, angleOffset);

                    bullet = Instantiate(prefab_Bullet, ctrler.transform.position, targetRotation);

                    bullet.GetComponent<EP_Bullet>().Initialize((int)(ctrler._enemyStatus.power * damageMultiple), 0);

                    await UniTask.Delay((int)(75), cancellationToken: token);
                }

                break;
        }

        await UniTask.Delay(1000, cancellationToken: token);
    }
}
