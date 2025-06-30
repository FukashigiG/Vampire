using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOnHitSpecialEffect
{

    bool dontDestroyBullet { get; }
    bool IgnoreDefence { get; }

    void OnHitSpecialEffect(Base_MobStatus status);
}
