using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class EP_Bomb : Base_EnemyProps
{
    [SerializeField] GameObject damageDetect;

    Vector2 targetPoint;
    float radius;

    public void Initialize_OR(Vector2 _targetPoint, float _radius, int damage)
    {
        targetPoint = _targetPoint;
        radius = _radius;

        base.Initialize(damage, 0);

        Task(transform.position, this.gameObject.GetCancellationTokenOnDestroy()).Forget();
    }

    async UniTaskVoid Task(Vector2 startPosi, CancellationToken token)
    {
        try
        {
            float moveTime = 0.9f;
            float elapsed = 0f;
            float jumpHeight = 9f;

            Vector2 dir = (targetPoint - startPosi);

            while (elapsed < moveTime)
            {
                elapsed += Time.deltaTime;

                // 0.0(開始) ～ 1.0(終了) の進行度を計算
                float t = Mathf.Clamp01(elapsed / moveTime);

                // A: 水平方向の移動（始点から終点へ滑らかに移動）
                Vector2 currentPos = Vector2.Lerp(startPosi, targetPoint, t);

                // B: 垂直方向の計算（放物線）
                // Mathf.Sin(t * Mathf.PI) は tが0で0、0.5で1、1で0になるカーブを描きます
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
