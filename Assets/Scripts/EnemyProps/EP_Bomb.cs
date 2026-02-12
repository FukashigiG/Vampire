using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class EP_Bomb : Base_EnemyProps
{
    [SerializeField] float radius = 1.5f;

    [SerializeField] GameObject damageDetect;

    float time_Impact;

    Vector2 targetPoint;


    public void Initialize_OR(Vector2 _targetPoint, float _time_Impact, int damage)
    {
        targetPoint = _targetPoint;

        time_Impact = _time_Impact;

        base.Initialize(damage, 0);

        Task(transform.position, this.gameObject.GetCancellationTokenOnDestroy()).Forget();
    }

    async UniTaskVoid Task(Vector2 startPosi, CancellationToken token)
    {
        try
        {
            float elapsed = 0f;
            float jumpHeight = 9f;

            Vector2 dir = (targetPoint - startPosi);

            while (elapsed < time_Impact)
            {
                elapsed += Time.deltaTime;

                // 0.0(開始) ～ 1.0(終了) の進行度を計算
                float t = Mathf.Clamp01(elapsed / time_Impact);

                // 水平方向の移動
                Vector2 currentPos = Vector2.Lerp(startPosi, targetPoint, t);

                // 垂直方向の計算（放物線）
                float yOffset = Mathf.Sin(t * Mathf.PI) * jumpHeight;

                // Y座標に高さを加算
                currentPos.y += yOffset;

                // 座標を更新
                transform.position = currentPos;

                await UniTask.Yield(cancellationToken: token);
            }

            GameObject x = Instantiate(damageDetect, targetPoint, Quaternion.identity);
            x.GetComponent<EP_Punch>().Initialie_OR(damage, elementDamage, AttackRangeType.circle, 0, size_Radius: radius);

            Destroy(this.gameObject);

        }
        catch
        {
            throw;
        }
    }
}
