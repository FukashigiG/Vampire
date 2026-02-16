using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using NaughtyAttributes;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBossEnemyAct", menuName = "Game Data/BossEnemyAct/Punch&Warp")]
public class BEA_PunchAndWarp : Base_BossEnemyAct
{
    [SerializeField] GameObject attackDetectObje;
    [SerializeField] GameObject warningPrefab;
    [SerializeField] float damageMultiplier;
    [SerializeField] float forwardDistance;

    AttackRangeType rangeType = AttackRangeType.box;

    [SerializeField] float size_Width = 0;
    [SerializeField] float size_Vertical = 0;

    public async override UniTask Action(EnemyCtrler_BigBoss ctrler, CancellationToken token)
    {
        // プレイヤーの方向を取得
        Vector2 dir = (ctrler.target.position - ctrler.transform.position).normalized;

        // 警告オブジェクトを生成
        GameObject warning = Instantiate(warningPrefab, ctrler.transform.position, Quaternion.FromToRotation(Vector2.up, dir));

        try
        {
            // 初期化、アニメーション終了まで待つ
            await warning.GetComponent<EP_Warning>().WarningAnim(delayTime, token, rangeType, forwardDistance, size_Width, size_Vertical);

            // こいつはキャンセル時はちゃんと呼ばれないっぽい
            //Debug.Log("afterAnim");
        }
        catch(OperationCanceledException)
        {
            // キャンセルされたら、警告を消して終了
            if (warning != null) Destroy(warning);
            throw; // キャンセル例外を上位に投げる
        }

        ctrler._animator.SetTrigger("Attack");

        // 本命の攻撃判定オブジェクトを生成、初期化
        GameObject x = Instantiate(attackDetectObje, ctrler.transform.position, Quaternion.FromToRotation(Vector2.up, dir));
        x.GetComponent<EP_Punch>().Initialie_OR((int)(ctrler._enemyStatus.power * damageMultiplier), 0, rangeType, forwardDistance, size_Width, size_Vertical);

        ctrler.gameObject.transform.Translate(dir * size_Vertical);

        await UniTask.Delay(700, cancellationToken: token);
    }
}
