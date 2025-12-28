using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBossEnemyAct", menuName = "Game Data/BossEnemyAct/Chase")]
public class BEA_Chase : Base_BossEnemyAct
{
    [Header("‰½•bŠÔ’ÇÕ‚·‚é‚©")]
    [SerializeField] float lifeTime_Action = 5f;
    [Header("’ÇÕ‚ğ’†’f‚·‚é‹——£")]
    [SerializeField] float distance_WrapUpAction = 4f;
 
    public async override UniTask Action(Base_EnemyCtrler ctrler, CancellationToken token)
    {
        float elapsedTime = 0f;

        Transform target = ctrler.target;

        while (elapsedTime < lifeTime_Action)
        {
            Vector2 dir = (target.position - ctrler.transform.position).normalized;

            ctrler.transform.Translate(dir * ctrler._enemyStatus.moveSpeed / 10 * Time.deltaTime);

            float distance = (target.position - ctrler.transform.position).magnitude;

            if (distance < distance_WrapUpAction) return;

            await UniTask.Delay((int)(1000 * Time.deltaTime), cancellationToken: token);

            elapsedTime += Time.deltaTime;
        }
    }
}
