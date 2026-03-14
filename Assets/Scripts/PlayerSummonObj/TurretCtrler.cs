using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UniRx;
using UnityEngine;

public class TurretCtrler : MonoBehaviour//, ISummonable
{
    [SerializeField] GameObject prefab_Bullet;

    [SerializeField] GameObject fx_Bullet;
    [SerializeField] GameObject fx_Shot;
    [SerializeField] GameObject fx_Die;

    [SerializeField] LayerMask targetLayer;

    // 寿命
    public float lifeTime { get; set; } = 50f;

    // 経過時間記録変数
    [SerializeField] float elapsedTime = 0;

    // 残弾数
    public int bulletNum = 20;

    // 発射間隔
    [SerializeField] float interval_Shot_Sec = 0.4f;

    // 感知射程
    [SerializeField] float eyeSight = 5.5f;

    // 弾の威力
    [SerializeField] int power = 8;

    [SerializeField] float bulletSpeed = 20f;

    // 生成された際に出す通知とその購読部分
    static Subject<TurretCtrler> subject_onAwake = new();
    public static IObservable<TurretCtrler> onAwake => subject_onAwake;

    // 消える際に出す通知とその購読部分
    static Subject<TurretCtrler> subject_onDestroy = new();
    public static IObservable<TurretCtrler> onDestroy => subject_onDestroy;

    CancellationToken token;

    bool onDie = false;

    void Awake()
    {
        // トークン取得
        token = this.GetCancellationTokenOnDestroy();

        // 通知の発行
        subject_onAwake.OnNext(this);

        ShotTask().Forget();
    }

    private void Update()
    {
        // 経過時間を加算
        elapsedTime += Time.deltaTime;

        // 寿命が来ていたら消える
        if (elapsedTime >= lifeTime)
        {
            Die();
        }
    }

    async UniTask ShotTask()
    {
        GameObject target = null;

        while (bulletNum > 0)
        {
            try
            {
                // クールタイム
                await UniTask.Delay((int)(interval_Shot_Sec * 1000), cancellationToken: token);

                // 攻撃対象が出現するまで待つ
                do
                {
                    target = FindEnemy();

                    await UniTask.Yield(cancellationToken: token);

                } while(target == null);
            }
            catch
            {

            }

            // キャンセルされてたらこの下は行わない
            token.ThrowIfCancellationRequested();


            // 何かしらのミスでいなかったらコンテ
            if (target == null) continue;

            // 攻撃対象の方向をVec2型で取得
            Vector2 dir = (target.transform.position - this.transform.position).normalized;

            // それをQuaternionに変換
            Quaternion rotation = Quaternion.FromToRotation(Vector2.up, dir);

            // 対象に向けて弾を生成
            GameObject bullet = Instantiate(prefab_Bullet, this.transform.position, rotation);

            // 攻撃力を弾に渡し、初期化処理
            bullet.GetComponent<PlayerPropsBulletCtrler>().Initialize(power, bulletSpeed, fx_Bullet);

            // 発射エフェクト
            Instantiate(fx_Shot, this.transform.position, Quaternion.identity);

            // 弾数消費
            bulletNum--;
        }

        try
        {
            await UniTask.Delay(500, cancellationToken: token);

        } catch { }

        Die();
    }

    GameObject FindEnemy()
    {

        //一定範囲内の敵を配列に格納
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, eyeSight, targetLayer);

        GameObject nearestObject = null;
        float shortestDistance = Mathf.Infinity; // 無限大で初期化

        // 一番近い敵を探索
        foreach (Collider2D hit in hits)
        {
            float Distance = Vector2.Distance(transform.position, hit.transform.position);

            if (Distance < shortestDistance)
            {
                shortestDistance = Distance;
                nearestObject = hit.gameObject;
            }
        }

        return nearestObject;
    }

    public void Die()
    {
        if (onDie) return;

        onDie = true;
 
        subject_onDestroy.OnNext(this);

        Instantiate(fx_Die, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
