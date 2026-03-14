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

    [SerializeField] float speedMuitiple = 2f;

    [SerializeField] float moveAmount;

    [SerializeField] int num_Bullet;

    [SerializeField] float damageMultiplier;

    [SerializeField] GameObject yokoku;

    [SerializeField] GameObject prefab_Bullet;
    [SerializeField] GameObject fx_Bullet;



    Transform target;

    public async override UniTask Action(EnemyCtrler_BigBoss ctrler, CancellationToken token)
    {
        

        target = ctrler.target;

        Vector2 dir = (target.position - ctrler.transform.position).normalized;

        // 突進する距離を決定
        // 進行方向に壁があればその本の手前で止まるように
        float _moveAmount = moveAmount;
        RaycastHit2D rayHit = Physics2D.Raycast(ctrler.transform.position, dir, _moveAmount, LayerMask.GetMask("Wall"));
        if (rayHit) _moveAmount = rayHit.distance * 0.95f;

        // 警告オブジェクトを生成
        GameObject warning = Instantiate(yokoku, ctrler.transform.position, Quaternion.FromToRotation(Vector2.up, dir));

        try
        {
            // アニメーション待機
            await warning.GetComponent<EP_Warning>().WarningAnim(delayTime, token, AttackRangeType.box, _moveAmount / 2, 3f, _moveAmount);
        }
        catch
        {
            if(warning != null) Destroy(warning);

            throw;
        }

        // ダメージ判定を子オブジェクトとして生成、初期化
        GameObject damageDetect = Instantiate(attackDetectObje, ctrler.gameObject.transform);
        damageDetect.GetComponent<EP_Punch>().Initialie_OR((int)(ctrler._enemyStatus.power * damageMultiplier), 0, AttackRangeType.box, 0, 2.1f, 2.1f, isInstant: false);

        GameObject bullet = null;
        float bulletTimer = 0f;
        float mileage = 0f;
        float _moveSpeed = ctrler._enemyStatus.moveSpeed / 10 * speedMuitiple;

        while (mileage < _moveAmount)
        {
            ctrler.transform.Translate(dir * _moveSpeed * Time.deltaTime);

            await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: token);

            mileage += _moveSpeed * Time.deltaTime;

            bulletTimer += _moveSpeed * Time.deltaTime;

            // 移動量が最長距離における弾一発を出すまでの距離を越えるたび弾を生成
            if(bulletTimer >= moveAmount / num_Bullet)
            {
                bulletTimer = 0f;

                // プレイヤーの方向を取得
                Vector2 shotdir = (ctrler.target.position - ctrler.transform.position).normalized;
                Quaternion _rotation = Quaternion.FromToRotation(Vector2.up, shotdir);

                // 弾を生成
                bullet = Instantiate(prefab_Bullet, ctrler.transform.position, _rotation);

                // 弾を初期化
                bullet.GetComponent<EP_Bullet>().Initialize((int)(ctrler._enemyStatus.power * damageMultiplier / 2), 0, fx_Bullet);
            }
        }

        Destroy(damageDetect);

        await UniTask.Delay(1000, cancellationToken: token);
    }
}
