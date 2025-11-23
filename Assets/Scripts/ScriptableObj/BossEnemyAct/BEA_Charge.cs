using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBossEnemyAct", menuName = "Game Data/BossEnemyAct/Charge")]
public class BEA_Charge : Base_BossEnemyAct
{
    // ìÀêi
    // í«ê’Ç∆à·Ç¢ÅAìríÜÇ≈ï˚å¸ì]ä∑Ç∆íÜífÇÇµÇ»Ç¢

    [SerializeField] float moveAmount;
    [SerializeField] float time;

    [SerializeField] GameObject yokoku;

    Transform target;

    public async override UniTask Action(Base_EnemyCtrler ctrler)
    {
        var token = ctrler.GetCancellationTokenOnDestroy();

        float elapsedTime = 0f;

        target = ctrler.target;

        Vector2 dir = (target.position - ctrler.transform.position).normalized;

        GameObject warning = Instantiate(yokoku, ctrler.transform.position, Quaternion.FromToRotation(Vector2.up, dir));

        await warning.GetComponent<EP_Warning>().WarningAnim(delayTime, token, AttackRangeType.box, moveAmount/2, 3f, moveAmount);

        while (elapsedTime < time)
        {
            ctrler.transform.Translate(dir * moveAmount / time * Time.deltaTime);

            await UniTask.Delay((int)(1000 * Time.deltaTime), cancellationToken: token);

            elapsedTime += Time.deltaTime;
        }

        await UniTask.Delay(1000, cancellationToken: token);
    }
}
