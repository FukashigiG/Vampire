using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using System.Threading;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpiritCtrler : MonoBehaviour
{
    [SerializeField] ParticleSystem fx;

    // 発射する弾のプレハブ
    [SerializeField] GameObject prefab_Bullet;

    // 弾の発射間隔（秒）
    public float interval_Shot_Sec = 1;

    // 攻撃力
    public int power = 4;

    // 寿命（秒）
    public float LifeTime = 10;

    // 周回半径
    [SerializeField] float radius = 1.2f;

    // 精霊自体の属性
    // 現時点では青固定とする
    Element element = Element.Blue;

    // 経過時間記録用変数
    float elapsedTime = 0;

    // 精霊が生まれた際に出す通知とその購読部分
    static Subject<SpiritCtrler> subject_onAwake = new Subject<SpiritCtrler>();
    public static IObservable<SpiritCtrler> onAwake => subject_onAwake;

    // 精霊が消える際に出す通知とその購読部分
    static Subject<SpiritCtrler> subject_onDestroy = new Subject<SpiritCtrler>();
    public static IObservable<SpiritCtrler> onDestroy => subject_onDestroy;

    float initialAngle;

    Transform player;

    CancellationToken token;

    void Awake()
    {
        // プレイヤーオブジェクトを取得、記憶
        player = PlayerController.Instance.transform;

        // 仕様変更:powerの値は固定

        // 自分の属性と被ってるプレイヤーのナイフの本数を取得
        /*power = PlayerController.Instance._status.inventory.runtimeKnives
            .Where(x => x.element == this.element)
            .Count();*/

        // 周回時の初期角度を決定
        initialAngle = Random.Range(0, 359);

        // トークン取得
        token = this.GetCancellationTokenOnDestroy();

        // 通知の発行
        subject_onAwake.OnNext(this);

        ShotTask().Forget();
    }

    void Update()
    {
        // 経過時間を加算
        elapsedTime += Time.deltaTime;

        // 寿命が来ていたら消える
        if(elapsedTime >= LifeTime)
        {
            Die();
        }

        // 周回角度の算出
        float radian = (elapsedTime * 90f + initialAngle) * Mathf.Deg2Rad;

        // 移動するべき場所の取得
        Vector2 targetPosi = (Vector2)player.position + new Vector2(Mathf.Cos(radian), Mathf.Sin(radian)) * radius;

        // 目標の場所と自分の座標の距離が長いほど移動速度を高くする
        float distance = (targetPosi - (Vector2)this.transform.position).magnitude;
        float moveSpeed = 3 * distance;
        // 速度の上限、下限
        moveSpeed = Mathf.Clamp(moveSpeed, 0.1f, 20);

        // 目的の方向
        Vector2 dir = (targetPosi - (Vector2)this.transform.position).normalized;

        // 移動命令
        transform.Translate(dir * moveSpeed * Time.deltaTime);
    }

    async UniTask ShotTask()
    {
        try
        {
            while (true)
            {
                // クールタイム
                await UniTask.Delay((int)(interval_Shot_Sec * 1000), cancellationToken: token);

                // 攻撃対象が出現するまで待つ
                await UniTask.WaitUntil(() => PlayerController.Instance._status.attack.targetEnemy != null, cancellationToken: token);

                // キャンセルされてたらこの下は行わない
                token.ThrowIfCancellationRequested();

                // 対象の取得
                var target = PlayerController.Instance._status.attack.targetEnemy;

                // 何かしらのミスでいなかったらコンテ
                if (target == null) continue;

                // 攻撃対象の方向をVec2型で取得
                Vector2 dir = (target.transform.position - this.transform.position).normalized;

                // それをQuaternionに変換
                Quaternion rotation = Quaternion.FromToRotation(Vector2.up, dir);

                // 対象に向けて弾を生成
                GameObject bullet = Instantiate(prefab_Bullet, this.transform.position, rotation);

                // 攻撃力を弾に渡し、初期化処理
                bullet.GetComponent<Bullet_SpilitCtrler>().Initialize(power);
            }
        }
        catch
        {

        }
    }

    // 消滅処理
    void Die()
    {
        // 下２行の処理について、OnDestroyないでやろうとするとなんかうまくいかない
        fx.transform.parent = null;
        fx.Stop();

        subject_onDestroy.OnNext(this);

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        
    }
}
