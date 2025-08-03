using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Unity.VisualScripting;

public class BossEnemyCtrler : Base_EnemyCtrler
{
    protected override void Awake()
    {
        base.Awake();

        EnemyStatus.onDie.Subscribe(x => onDie(x.status)).AddTo(this);
    }

    void onDie(Base_MobStatus stat)
    {
        if (stat != _enemyStatus) return;

        GameEventDirector.Instance.TriggerEvent(GameEventDirector.Events.getTreasure);
    }
}
