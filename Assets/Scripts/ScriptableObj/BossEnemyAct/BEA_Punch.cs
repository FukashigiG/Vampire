using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBossEnemyAct", menuName = "Game Data/BossEnemyAct/Punch")]
public class BEA_Punch : Base_BossEnemyAct
{
    [SerializeField] GameObject attackDetectObje;
    [SerializeField] int baseDamage;
    [SerializeField] int elementDamage;

    public async override UniTask Action(Base_EnemyCtrler ctrler)
    {
        var token = ctrler.GetCancellationTokenOnDestroy();

        await UniTask.Delay(500, cancellationToken: token);

        Vector2 dir = (ctrler.target.position - ctrler.transform.position).normalized;

        GameObject x = Instantiate(attackDetectObje, ctrler.transform.position, Quaternion.FromToRotation(Vector2.up, dir));

        x.GetComponent<Base_EnemyProps>().Initialie(baseDamage, elementDamage);

        await UniTask.Delay(1000, cancellationToken: token);
    }
}
