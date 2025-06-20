using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : Base_MobStatus
{
    [SerializeField] EnemyData _enemyData;

    protected override void Start()
    {
        base.Start();

        hitPoint = _enemyData.hp;
    }
}
