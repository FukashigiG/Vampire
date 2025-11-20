using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UniRx;

public class EnemyCtrler_Shooter : Base_EnemyCtrler
{
    // 遠距離射撃型

    // Updateにて使用される変数　それ以外のとこでは参照のみにしてね
    // 処理負荷軽減のため外に出してるよ
    float distance;
    Vector2 dir;

    bool attackable = true;

    CancellationToken token;

    protected override void Awake()
    {
        base.Awake();

        token = gameObject.GetCancellationTokenOnDestroy();
    }

    protected override void HandleAI()
    {
        // 射程内でなければ接近し、射程内なら攻撃

        // 目的の距離と方向を取得
        distance = (target.position - this.transform.position).magnitude;
        dir = (target.position - this.transform.position).normalized;

        // 攻撃対象が射程内なら発射、そうでないなら移動
        if(distance <= _enemyStatus.range_Shot)
        {
            // 攻撃可能状態でなければ待機
            if (! attackable) return;

            // 角度を求める
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            Instantiate(_enemyStatus.bullet_Prefab, transform.position, Quaternion.FromToRotation(Vector2.up, dir));

            attackable = false;

            StandBy(token).Forget();
        }
        else
        {
            transform.Translate(dir * _enemyStatus.moveSpeed / 10f * Time.fixedDeltaTime);
        }
    }

    async UniTask StandBy(CancellationToken token)
    {
        await UniTask.Delay((int)(_enemyStatus.friquentry_Shot * 1000), cancellationToken: token);

        attackable = true;
    }
}
