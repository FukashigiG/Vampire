using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOnHitSpecialEffect
{

    bool dontDestroyBullet { get; }
    bool ignoreDefence { get; }
    bool critical { get; }

    void OnHitSpecialEffect(Base_MobStatus status, Vector2 position, KnifeData knifeData);
}
