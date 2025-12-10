using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using NaughtyAttributes;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBossEnemyAct", menuName = "Game Data/BossEnemyAct/Punch")]
public class BEA_Punch : Base_BossEnemyAct
{
    [SerializeField] GameObject attackDetectObje;
    [SerializeField] GameObject warningPrefab;
    [SerializeField] float damageMultiplier;
    [SerializeField] float forwardDistance;

    [SerializeField] AttackRangeType rangeType;

    bool isBox => rangeType == AttackRangeType.box;
    bool isCircle => rangeType == AttackRangeType.circle;

    [ShowIf("isBox"), SerializeField] float size_Width = 0;
    [ShowIf("isBox"), SerializeField] float size_Vertical = 0;

    [ShowIf("isCircle"), SerializeField] float size_Radius = 0;

    public async override UniTask Action(Base_EnemyCtrler ctrler, CancellationToken token)
    {
        // プレイヤーの方向を取得
        Vector2 dir = (ctrler.target.position - ctrler.transform.position).normalized;

        // 警告オブジェクトを生成
        GameObject warning = Instantiate(warningPrefab, ctrler.transform.position, Quaternion.FromToRotation(Vector2.up, dir));

        try
        {
            // 初期化、アニメーション終了まで待つ
            await warning.GetComponent<EP_Warning>().WarningAnim(delayTime, token, rangeType, forwardDistance, size_Width, size_Vertical, size_Radius);
        }
        catch(OperationCanceledException)
        {
            Debug.Log("??????");

            // キャンセルされたら（Destroyされたら）、警告を消して終了
            //if (warning != null) Destroy(warning);
            throw; // キャンセル例外を上位（Controller）に投げる
        }

        //token.ThrowIfCancellationRequested();

        // 本命の攻撃判定オブジェクトを生成、初期化
        GameObject x = Instantiate(attackDetectObje, ctrler.transform.position, Quaternion.FromToRotation(Vector2.up, dir));
        x.GetComponent<EP_Punch>().Initialie_OR((int)(ctrler._enemyStatus.power * damageMultiplier), 0, rangeType, forwardDistance, size_Width, size_Vertical, size_Radius);

        await UniTask.Delay(1000, cancellationToken: token);
    }
}
