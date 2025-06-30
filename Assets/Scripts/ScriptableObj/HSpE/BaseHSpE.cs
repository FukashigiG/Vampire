using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHSpE", menuName = "Game Data/zzzDont")]
public class BaseHSpE : ScriptableObject, IOnHitSpecialEffect
{
    public virtual bool DestroyBullet
    {
        get { return true; }
    }

    public virtual void OnHitSpecialEffect(Base_MobStatus status)
    {

    }
}
