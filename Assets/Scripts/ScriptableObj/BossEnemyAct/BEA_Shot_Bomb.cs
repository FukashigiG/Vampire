using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "NewBossEnemyAct", menuName = "Game Data/BossEnemyAct/Basic/ShotBomb")]
public class BEA_Shot_Bomb : Base_BossEnemyAct
{
    // ƒ{ƒ€’e”­Ëˆ—

    [SerializeField] GameObject prefab_Bullet;
    [SerializeField] int num_Bullet;
    [SerializeField] float damageMultiple = 0.5f;
    [SerializeField] float randomRadius = 2f;

    public async override UniTask Action(EnemyCtrler_BigBoss ctrler, CancellationToken token)
    {
        // ƒvƒŒƒCƒ„[‚Ì•ûŒü‚ğæ“¾
        Vector2 dir = (ctrler.target.position - ctrler.transform.position).normalized;

        GameObject bullet = null;

        ctrler._animator.SetTrigger("Spell");

        Vector2 playerPoint = PlayerController.Instance.transform.position;

        for (int i = 0; i < num_Bullet; i++)
        {
            Vector2 target = playerPoint + Random.insideUnitCircle * randomRadius;

            // ’e‚ğ¶¬
            bullet = Instantiate(prefab_Bullet, ctrler.transform.position, Quaternion.identity);

            // ’e‚ğ‰Šú‰»
            bullet.GetComponent<EP_Bomb>().Initialize_OR(target, 2f, ctrler._enemyStatus.power);
        }

        await UniTask.Delay(1000, cancellationToken: token);
    }
}
