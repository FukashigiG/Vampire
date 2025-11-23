using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
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

    public async override UniTask Action(Base_EnemyCtrler ctrler)
    {
        var token = ctrler.GetCancellationTokenOnDestroy();

        // プレイヤーの方向を取得
        Vector2 dir = (ctrler.target.position - ctrler.transform.position).normalized;

        // 警告オブジェクトを生成、初期化、アニメーション終了まで待つ
        GameObject warning = Instantiate(warningPrefab, ctrler.transform.position, Quaternion.FromToRotation(Vector2.up, dir));
        await warning.GetComponent<EP_Warning>().WarningAnim(delayTime, token, rangeType, forwardDistance, size_Width, size_Vertical, size_Radius);

        // 本命の攻撃判定オブジェクトを生成、初期化
        GameObject x = Instantiate(attackDetectObje, ctrler.transform.position, Quaternion.FromToRotation(Vector2.up, dir));
        x.GetComponent<EP_Punch>().Initialie_OR((int)(ctrler._enemyStatus.power * damageMultiplier), 0, rangeType, forwardDistance, size_Width, size_Vertical, size_Radius);

        await UniTask.Delay(1000, cancellationToken: token);
    }
}
