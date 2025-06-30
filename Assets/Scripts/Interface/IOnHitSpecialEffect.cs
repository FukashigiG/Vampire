using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOnHitSpecialEffect
{

    bool DestroyBullet { get; }

    void OnHitSpecialEffect(Base_MobStatus status);
}
