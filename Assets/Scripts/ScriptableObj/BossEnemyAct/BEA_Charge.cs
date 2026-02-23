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
    [SerializeField] float time;

    [SerializeField] float damageMultiplier;

    [SerializeField] GameObject yokoku;

    Transform target;

    public async override UniTask Action(EnemyCtrler_BigBoss ctrler, CancellationToken token)
    {
        float elapsedTime = 0f;

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
            if (warning != null) Destroy(warning);

            throw;
        }
        // ダメージ判定を子オブジェクトとして生成、初期化
        GameObject damageDetect = Instantiate(attackDetectObje, ctrler.gameObject.transform);
        damageDetect.GetComponent<EP_Punch>().Initialie_OR((int)(ctrler._enemyStatus.power * damageMultiplier), 0, AttackRangeType.box, 0, 1.1f, 1.1f, isInstant: false);

        while (elapsedTime < time)
        {
            ctrler.transform.Translate(dir * moveAmount / time * Time.deltaTime);

            await UniTask.Delay((int)(1000 * Time.deltaTime), cancellationToken: token);

            elapsedTime += Time.deltaTime;
        }

        Destroy(damageDetect);

        await UniTask.Delay(1000, cancellationToken: token);
    }
}
