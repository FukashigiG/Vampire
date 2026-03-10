using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBossEnemyAct", menuName = "Game Data/BossEnemyAct/Basic/Charge")]
public class BEA_Charge : Base_BossEnemyAct
{
    // 突進
    // 追跡と違い、途中で方向転換と中断をしない

    [SerializeField] GameObject attackDetectObje;

    [SerializeField] float moveAmount;

    [SerializeField] float speedMuitiple = 2f;

    [SerializeField] float damageMultiplier;

    [SerializeField] GameObject yokoku;

    Transform target;

    public async override UniTask Action(EnemyCtrler_BigBoss ctrler, CancellationToken token)
    {
        target = ctrler.target;

        Vector2 dir = (target.position - ctrler.transform.position).normalized;

        // 突進する距離を決定
        // 進行方向に壁があればその本の手前で止まるように
        float _moveAmount = moveAmount;
        RaycastHit2D rayHit = Physics2D.Raycast(ctrler.transform.position, dir, _moveAmount, LayerMask.GetMask("Wall"));
        if(rayHit) _moveAmount = rayHit.distance * 0.95f;

        // 警告オブジェクトを生成
        GameObject warning = Instantiate(yokoku, ctrler.transform.position, Quaternion.FromToRotation(Vector2.up, dir));

        try
        {
            // アニメーション待機
            await warning.GetComponent<EP_Warning>().WarningAnim(delayTime, token, AttackRangeType.box, _moveAmount / 2, 3f, _moveAmount);
        }
        catch
        {
            if (warning != null) Destroy(warning);

            throw;
        }
        // ダメージ判定を子オブジェクトとして生成、初期化
        GameObject damageDetect = Instantiate(attackDetectObje, ctrler.gameObject.transform);
        damageDetect.GetComponent<EP_Punch>().Initialie_OR((int)(ctrler._enemyStatus.power * damageMultiplier), 0, AttackRangeType.box, 0, 2.1f, 2.1f, isInstant: false);

        float mileage = 0f;
        float _moveSpeed = ctrler._enemyStatus.moveSpeed / 10 * speedMuitiple;

        while (mileage < _moveAmount)
        {
            ctrler.transform.Translate(dir * _moveSpeed * Time.deltaTime);

            await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: token);

            mileage += _moveSpeed * Time.deltaTime;
        }

        Destroy(damageDetect);

        await UniTask.Delay(1000, cancellationToken: token);
    }
}
