using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOnHitSpecialEffect
{

    bool dontDestroyBullet { get; }
    bool ignoreDefence { get; }
    bool critical { get; }

    void OnHit(Base_MobStatus status, Vector2 position, KnifeData_RunTime knifeData);
}
