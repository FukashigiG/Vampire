using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using System.Threading;
using System;
using UnityEngine;
using Unity.VisualScripting;

[CreateAssetMenu(fileName = "NewBossEnemyAct", menuName = "Game Data/BossEnemyAct/ContinuousPunches")]
public class BEA_ContinuousPunches : Base_BossEnemyAct
{
    [SerializeField] GameObject attackDetectObje;
    [SerializeField] GameObject warningPrefab;
    [SerializeField] float damageMultiplier;

    [SerializeField] float size_Radius = 0;

    [SerializeField] int num_Attacks = 0;

    [SerializeField] float sizeRate_Percent = 0f;

    [SerializeField] float distance = 0f;

    public async override UniTask Action(EnemyCtrler_BigBoss ctrler, CancellationToken token)
    {
        // プレイヤーの方向を取得
        Vector2 dir = (ctrler.target.position - ctrler.transform.position).normalized;

        for (int i = 0; i < num_Attacks; i++)
        {
            Vector2 pos = (Vector2)ctrler.transform.position + dir * distance * i;

            float size = size_Radius * (1 + sizeRate_Percent / 100 * i);

            Attack(ctrler, pos, size, token).Forget();

            await UniTask.Delay(200, cancellationToken: token);
        }

        ctrler._animator.SetTrigger("Attack");

        await UniTask.Delay((int)(1000 * delayTime), cancellationToken: token);
    }

    async UniTaskVoid Attack(Base_EnemyCtrler ctrler, Vector2 pos, float radius, CancellationToken token)
    {
        // 警告オブジェクトを生成
        GameObject warning = Instantiate(warningPrefab, pos, Quaternion.identity);

        try
        {
            // 初期化、アニメーション終了まで待つ
            await warning.GetComponent<EP_Warning>().WarningAnim(delayTime, token, AttackRangeType.circle, 0, size_Range: radius);

            // こいつはキャンセル時はちゃんと呼ばれないっぽい
            //Debug.Log("afterAnim");
        }
        catch (OperationCanceledException)
        {
            // キャンセルされたら、警告を消して終了
            if (warning != null) Destroy(warning);
            throw; // キャンセル例外を上位に投げる
        }

        // 本命の攻撃判定オブジェクトを生成、初期化
        GameObject x = Instantiate(attackDetectObje, pos, Quaternion.identity);
        x.GetComponent<EP_Punch>().Initialie_OR((int)(ctrler._enemyStatus.power * damageMultiplier), 0, AttackRangeType.circle, 0, size_Radius: radius);

    }
}
