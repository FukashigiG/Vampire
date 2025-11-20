using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch_BossProps : Base_EnemyProps
{
    [SerializeField] GameObject fx_Child;

    public override void Initialie(int dmg, int elementDmg)
    {
        base.Initialie(dmg, elementDmg);

        fx_Child.transform.parent = null;

        WaitAndDestroy().Forget();
    }

    async UniTask WaitAndDestroy()
    {
        await UniTask.Delay(500, cancellationToken: this.GetCancellationTokenOnDestroy());

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
