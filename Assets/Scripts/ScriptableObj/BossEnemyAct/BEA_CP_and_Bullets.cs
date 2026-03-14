using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using System.Threading;
using System;
using UnityEngine;
using Unity.VisualScripting;

[CreateAssetMenu(fileName = "NewBossEnemyAct", menuName = "Game Data/BossEnemyAct/CP&Bullets")]
public class BEA_CP_and_Bullets : Base_BossEnemyAct
{
    [SerializeField] GameObject attackDetectObje;
    [SerializeField] GameObject warningPrefab;
    [SerializeField] GameObject bullet_Prefab;
    [SerializeField] GameObject fx;
    [SerializeField] GameObject fx_Bullet;
    [SerializeField] float damageMultiplier;

    [SerializeField] float size_Radius = 0;

    [SerializeField] int base_Num_Attacks = 0;

    [SerializeField] float sizeRate_Percent = 0f;

    [SerializeField] float distance = 0f;

    [SerializeField] int num_Bullet_PerRadius;

    public async override UniTask Action(EnemyCtrler_BigBoss ctrler, CancellationToken token)
    {
        // プレイヤーの方向を取得
        Vector2 dir = (ctrler.target.position - ctrler.transform.position).normalized;

        // 攻撃判定の数をけってい
        // 残りHPが少ないほど増える
        int num_Attacks = (int)(base_Num_Attacks * (1f + (1f - (float)ctrler._enemyStatus.hitPoint.Value / (float)ctrler._enemyStatus.maxHP)));

        for (int i = 0; i < num_Attacks; i++)
        {
            Vector2 pos = (Vector2)ctrler.transform.position + dir * distance * i;

            float size = size_Radius * (1 + sizeRate_Percent / 100 * i);

            Attack(ctrler, pos, size, (i != 0), token).Forget();

            await UniTask.Delay(200, cancellationToken: token);
        }

        ctrler._animator.SetTrigger("Attack");

        await UniTask.Delay((int)(1000 * delayTime), cancellationToken: token);
    }

    async UniTaskVoid Attack(Base_EnemyCtrler ctrler, Vector2 pos, float radius, bool withBullet, CancellationToken token)
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

        // エフェクト生成
        GameObject y = Instantiate(fx, pos,Quaternion.identity);
        y.transform.localScale = new Vector3(radius * 2, radius * 2, 1f);

        int num_Bullets = (int)(radius * num_Bullet_PerRadius);

        GameObject bullet;

        if (withBullet)
        {
            // 弾
            for (int i = 0; i < radius * num_Bullet_PerRadius; i++)
            {
                Quaternion r = Quaternion.Euler(0, 0, 360f * i / num_Bullets);

                // 弾を生成
                bullet = Instantiate(bullet_Prefab, pos, r);
                // 弾を初期化
                bullet.GetComponent<EP_Bullet>().Initialize((int)(ctrler._enemyStatus.power * damageMultiplier * 0.7f), 0, fx_Bullet);
            }
        }
    }
}
