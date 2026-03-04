using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using System.Threading;
using System;
using UnityEngine;
using Unity.VisualScripting;

[CreateAssetMenu(fileName = "NewBossEnemyAct", menuName = "Game Data/BossEnemyAct/ContinuousSlash")]
public class BEA_ContinuousSlash : Base_BossEnemyAct
{
    [SerializeField] GameObject attackDetectObje;
    [SerializeField] GameObject warningPrefab;
    [SerializeField] GameObject fx;
    [SerializeField] float damageMultiplier;

    [SerializeField] float size_Width = 2;
    [SerializeField] float size_Length = 7;

    [SerializeField] int num_Attacks = 0;

    public async override UniTask Action(EnemyCtrler_BigBoss ctrler, CancellationToken token)
    {
        float randomAngle = UnityEngine.Random.Range(0f, 360f);

        for (int i = 0; i < num_Attacks; i++)
        {
            Vector2 pos = ctrler.target.transform.position;

            float baseAngle = 360 * i / num_Attacks;

            Attack(ctrler, pos, Quaternion.Euler(0, 0, baseAngle + randomAngle), token).Forget();

            await UniTask.Delay(200, cancellationToken: token);
        }

        ctrler._animator.SetTrigger("Attack");

        await UniTask.Delay((int)(1000 * delayTime), cancellationToken: token);
    }

    async UniTaskVoid Attack(Base_EnemyCtrler ctrler, Vector2 pos, Quaternion angle, CancellationToken token)
    {
        // 警告オブジェクトを生成
        GameObject warning = Instantiate(warningPrefab, pos, angle);

        try
        {
            // 初期化、アニメーション終了まで待つ
            await warning.GetComponent<EP_Warning>().WarningAnim(delayTime, token, AttackRangeType.box, 0, size_X:size_Width, size_Y:size_Length);
        }
        catch (OperationCanceledException)
        {
            // キャンセルされたら、警告を消して終了
            if (warning != null) Destroy(warning);
            throw; // キャンセル例外を上位に投げる
        }

        // 本命の攻撃判定オブジェクトを生成、初期化
        GameObject x = Instantiate(attackDetectObje, pos, angle);
        x.GetComponent<EP_Punch>().Initialie_OR((int)(ctrler._enemyStatus.power * damageMultiplier), 0, AttackRangeType.box, 0, size_X: size_Width, size_Y: size_Length);

        GameObject y = Instantiate(fx, pos, angle);
    }
}
