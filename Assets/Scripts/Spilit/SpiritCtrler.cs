using Cysharp.Threading.Tasks;
using System.Linq;
using System.Threading;
using UnityEngine;

public class SpiritCtrler : MonoBehaviour
{
    [SerializeField] GameObject prefab_Bullet;

    float interval_Shot_Sec = 1;

    float LifeTime = 15;

    float elapsedTime = 0;

    float initialAngle;

    float radius = 1.2f;

    Transform player;

    CancellationToken token;

    Element element = Element.Blue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = PlayerController.Instance.transform;

        initialAngle = Random.Range(0, 359);

        token = this.GetCancellationTokenOnDestroy();

        ShotTask().Forget();
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        if(elapsedTime >= LifeTime)
        {
            Destroy(gameObject);
        }

        float radian = (elapsedTime * 90f + initialAngle) * Mathf.Deg2Rad;

        Vector2 targetPosi = (Vector2)player.position + new Vector2(Mathf.Cos(radian), Mathf.Sin(radian)) * radius;

        float distance = (targetPosi - (Vector2)this.transform.position).magnitude;

        Vector2 dir = (targetPosi - (Vector2)this.transform.position).normalized;

        float moveSpeed = 3 * distance;

        moveSpeed = Mathf.Clamp(moveSpeed, 0.1f, 20);

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

                // 自分の属性と被ってるプレイヤーのナイフの本数を取得
                int x = PlayerController.Instance._status.inventory.runtimeKnives
                    .Where(x => x.element == this.element)
                    .Count();

                // その数字を弾に渡す
                bullet.GetComponent<Bullet_SpilitCtrler>().Initialize(x);
            }
        }
        catch
        {

        }
    }
}
