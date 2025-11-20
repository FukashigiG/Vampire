using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UniRx;
using System.Linq;

public class EnemyCtrler_Boss : Base_EnemyCtrler
{
    // ボスの行動

    [SerializeField] List<Base_BossEnemyAct> actions;

    bool attackable = true;

    CancellationToken token;

    protected override void Awake()
    {
        base.Awake();

        token = gameObject.GetCancellationTokenOnDestroy();

        LifeCycle().Forget();
    }

    protected override void FixedUpdate()
    {
    }

    protected override void HandleAI()
    {
    }

    async UniTask LifeCycle()
    {
        while (true)
        {
            Base_BossEnemyAct act = DecideNextAction();

            await act.Action(this);
        }
    }

    Base_BossEnemyAct DecideNextAction()
    {
        float distance = (target.position - this.transform.position).magnitude;

        // 射程内の行動を取得
        List<Base_BossEnemyAct> a = actions.Where(x => x.range >= distance).ToList();

        return a[0];
    }
}
