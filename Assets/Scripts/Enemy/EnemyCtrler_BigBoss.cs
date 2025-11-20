using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UniRx;
using System.Linq;

public class EnemyCtrler_BigBoss : Base_EnemyCtrler
{
    // ボスの行動

    [SerializeField] List<Base_BossEnemyAct> actions;

    bool attackable = true;

    CancellationToken token;

    protected override void Awake()
    {
        base.Awake();

        token = gameObject.GetCancellationTokenOnDestroy();
    }

    protected override void Start()
    {
        base.Start();

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

            await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: token);
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
