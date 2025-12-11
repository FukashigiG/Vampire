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
            Quaternion baseRotation = Quaternion.FromToRotation(Vector2.up, dir);

            attackable = false;

            ShotTask(baseRotation, token).Forget();
        }
        else
        {
            transform.Translate(dir * _enemyStatus.moveSpeed / 10f * Time.fixedDeltaTime);
        }
    }

    async UniTask ShotTask(Quaternion baseRotation, CancellationToken token)
    {
        float angleOffset = 0;
        GameObject bullet = null;

        switch (_enemyStatus.shotType)
        {
            // 単射タイプの挙動
            case EnemyData.ShotType.OneShot:

                for (int i = 0; i < _enemyStatus.num_Bullet; i++)
                {
                    // 弾の数が２以上なら以下の計算を実行
                    if (_enemyStatus.num_Bullet > 1)
                    {
                        // 今投げる角度を求める
                        angleOffset = Mathf.Lerp(_enemyStatus.divergenceAngle * -1 / 2, _enemyStatus.divergenceAngle / 2, (float)i / (_enemyStatus.num_Bullet - 1));
                    }

                    // Quaternionに変換
                    Quaternion rotationOffset = Quaternion.Euler(0, 0, angleOffset);

                    // ベースの方向と合成
                    Quaternion finalRotation = baseRotation * rotationOffset;

                    // 弾を生成
                    bullet = Instantiate(_enemyStatus.bullet_Prefab, this.transform.position, finalRotation);

                    // 弾を初期化
                    bullet.GetComponent<EP_Bullet>().Initialize(1, 0);
                }

                break;

            // 連射タイプの挙動
            case EnemyData.ShotType.RapidFire:

                Quaternion targetRotation;

                // 中心角度からランダムにずらして弾をばらまく
                for(int i = 0; i < _enemyStatus.num_Bullet; i++)
                {
                    angleOffset = Random.Range(_enemyStatus.divergenceAngle * -1 / 2, _enemyStatus.divergenceAngle / 2);

                    targetRotation = baseRotation * Quaternion.Euler(0, 0, angleOffset);

                    bullet = Instantiate(_enemyStatus.bullet_Prefab, transform.position, targetRotation);

                    bullet.GetComponent<EP_Bullet>().Initialize(1, 0);

                    await UniTask.Delay((int)(75), cancellationToken: token);
                }

                break;
        }

        await UniTask.Delay((int)(_enemyStatus.friquentry_Shot * 1000), cancellationToken: token);

        attackable = true;
    }
}
