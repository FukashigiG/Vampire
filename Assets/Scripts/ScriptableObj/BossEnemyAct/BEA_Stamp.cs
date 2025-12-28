using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using NaughtyAttributes;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBossEnemyAct", menuName = "Game Data/BossEnemyAct/Stamp")]
public class BEA_Stamp : Base_BossEnemyAct
{
    // スタンプ攻撃の処理

    [SerializeField] GameObject attackDetectObje;
    [SerializeField] GameObject warningPrefab;
    [SerializeField] float damageMultiplier;
    [SerializeField] float radius;

    public async override UniTask Action(Base_EnemyCtrler ctrler, CancellationToken token)
    {
        // プレイヤーの位置を取得
        Vector2 targetPosi = ctrler.target.position;

        // ボスを非表示にしたり攻撃を受け付けなくする処理

        // 警告オブジェクトを生成
        GameObject warning = Instantiate(warningPrefab, targetPosi, Quaternion.identity);

        try
        {
            // 初期化、アニメーション終了まで待つ
            await warning.GetComponent<EP_Warning>().WarningAnim(delayTime, token, AttackRangeType.circle, 0, size_Range: radius);

            // 警告が終わったらジャンプアニメーション

            ctrler._enemyStatus.count_PermissionHit.Value++;

            float moveTime = 0.9f;
            float elapsed = 0f;
            float jumpHeight = 9f;

            Vector2 startPosi = ctrler.transform.position;

            Vector2 dir = (targetPosi - startPosi);

            while(elapsed < moveTime)
            {
                elapsed += Time.deltaTime;

                // 0.0(開始) ～ 1.0(終了) の進行度を計算
                float t = Mathf.Clamp01(elapsed / moveTime);

                // A: 水平方向の移動（始点から終点へ滑らかに移動）
                Vector2 currentPos = Vector2.Lerp(startPosi, targetPosi, t);

                // B: 垂直方向の計算（放物線）
                // Mathf.Sin(t * Mathf.PI) は tが0で0、0.5で1、1で0になるカーブを描きます
                float yOffset = Mathf.Sin(t * Mathf.PI) * jumpHeight;

                // Y座標に高さを加算
                currentPos.y += yOffset;

                // 座標を更新
                ctrler.transform.position = currentPos;

                await UniTask.Yield(cancellationToken: token);
            }

            // こいつはキャンセル時はちゃんと呼ばれないっぽい
            //Debug.Log("afterAnim");
        }
        catch (OperationCanceledException)
        {
            // キャンセルされたら、警告を消して終了
            if (warning != null) Destroy(warning);

            // ボスの非表示とかを解除する
            ctrler._enemyStatus.count_PermissionHit.Value--;

            throw; // キャンセル例外を上位に投げる
        } 

        // 本命の攻撃判定オブジェクトを生成、初期化
        GameObject x = Instantiate(attackDetectObje, targetPosi, Quaternion.identity);
        x.GetComponent<EP_Punch>().Initialie_OR((int)(ctrler._enemyStatus.power * damageMultiplier), 0, AttackRangeType.circle, 0, size_Radius:radius, isInstant:true);

        // 目標の場所に移動(念のため)
        ctrler.transform.position = targetPosi;

        // ボスの非表示とかを解除する
        ctrler._enemyStatus.count_PermissionHit.Value--;

        await UniTask.Delay(500, cancellationToken: token);
    }
}
