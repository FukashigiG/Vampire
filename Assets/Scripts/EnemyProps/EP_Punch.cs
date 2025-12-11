using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EP_Punch : Base_EnemyProps
{
    [SerializeField] BoxCollider2D boxCollider;
    [SerializeField] CircleCollider2D circleCollider;

    public void Initialie_OR(int dmg, int elementDmg, AttackRangeType rangeType, float forwardDistance, float size_X = 0, float size_Y = 0, float size_Radius = 0)
    {
        base.Initialize(dmg, elementDmg);

        switch (rangeType)
        {
            case AttackRangeType.box:
                boxCollider.size = new Vector2 (size_X, size_Y);
                boxCollider.offset = new Vector2 (0, forwardDistance);
                boxCollider.enabled = true;
                break;

            case AttackRangeType.circle:
                circleCollider.radius = size_Radius;
                circleCollider.offset = new Vector2 (0, forwardDistance);
                circleCollider.enabled = true;
                break;

            default:
                
                break;
        }

        Action().Forget();
    }

    async UniTask Action()
    {
        await UniTask.Delay(200, cancellationToken: this.GetCancellationTokenOnDestroy());

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // もし当たったものがダメージを受けるものだったらダメージを与える
        if (collision.gameObject.TryGetComponent(out Base_MobStatus ms))
        {
            ms?.GetAttack(damage, elementDamage, transform.position);
        }
    }
}
