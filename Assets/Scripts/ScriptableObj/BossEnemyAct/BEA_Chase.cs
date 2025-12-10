using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBossEnemyAct", menuName = "Game Data/BossEnemyAct/Chase")]
public class BEA_Chase : Base_BossEnemyAct
{
    [SerializeField] float speed;

    Transform target;

    public async override UniTask Action(Base_EnemyCtrler ctrler, CancellationToken token)
    {
        float elapsedTime = 0f;

        target = ctrler.target;

        while (elapsedTime < 5f)
        {
            Vector2 dir = (target.position - ctrler.transform.position).normalized;

            ctrler.transform.Translate(dir * speed * Time.deltaTime);

            float distance = (target.position - ctrler.transform.position).magnitude;

            if (distance < 2f) return;

            await UniTask.Delay((int)(1000 * Time.deltaTime), cancellationToken: token);

            elapsedTime += Time.deltaTime;
        }
    }
}
