using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBossEnemyAct", menuName = "Game Data/BossEnemyAct/ChargeAndShot")]
public class BEA_ChargeAndShot : Base_BossEnemyAct
{
    // 突進しつつ、弾を発射
    // 追跡と違い、途中で方向転換と中断をしない

    [SerializeField] GameObject attackDetectObje;

    [SerializeField] float moveAmount;
    [SerializeField] float time;

    [SerializeField] int num_Bullet;

    [SerializeField] float damageMultiplier;

    [SerializeField] GameObject yokoku;

    [SerializeField] GameObject prefab_Bullet;



    Transform target;

    public async override UniTask Action(EnemyCtrler_BigBoss ctrler, CancellationToken token)
    {
        

        target = ctrler.target;

        Vector2 dir = (target.position - ctrler.transform.position).normalized;

        // 警告オブジェクトを生成
        GameObject warning = Instantiate(yokoku, ctrler.transform.position, Quaternion.FromToRotation(Vector2.up, dir));

        try
        {
            // アニメーション待機
            await warning.GetComponent<EP_Warning>().WarningAnim(delayTime, token, AttackRangeType.box, moveAmount / 2, 3f, moveAmount);
        }
        catch
        {
            if(warning != null) Destroy(warning);

            throw;
        }

        // ダメージ判定を子オブジェクトとして生成、初期化
        GameObject damageDetect = Instantiate(attackDetectObje, ctrler.gameObject.transform);
        damageDetect.GetComponent<EP_Punch>().Initialie_OR((int)(ctrler._enemyStatus.power * damageMultiplier), 0, AttackRangeType.box, 0, 1.1f, 1.1f, isInstant: false);

        GameObject bullet = null;
        float bulletTimer = 0f;
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            ctrler.transform.Translate(dir * moveAmount / time * Time.deltaTime);

            await UniTask.Delay((int)(1000 * Time.deltaTime), cancellationToken: token);

            elapsedTime += Time.deltaTime;

            bulletTimer += Time.deltaTime;

            if(bulletTimer >= time / num_Bullet)
            {
                bulletTimer = 0f;

                // プレイヤーの方向を取得
                Vector2 shotdir = (ctrler.target.position - ctrler.transform.position).normalized;
                Quaternion _rotation = Quaternion.FromToRotation(Vector2.up, shotdir);

                // 弾を生成
                bullet = Instantiate(prefab_Bullet, ctrler.transform.position, _rotation);

                // 弾を初期化
                bullet.GetComponent<EP_Bullet>().Initialize((int)(ctrler._enemyStatus.power * damageMultiplier / 2), 0);
            }
        }

        Destroy(damageDetect);

        await UniTask.Delay(1000, cancellationToken: token);
    }
}
