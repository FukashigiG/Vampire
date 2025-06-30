using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHSpE", menuName = "Game Data/zzzDont")]
public class BaseHSpE : ScriptableObject, IOnHitSpecialEffect
{
    public string effectName;

    public virtual bool DestroyBullet
    {
        get { return true; }
    }

    public virtual bool IgnoreDefence
    {
        get { return false; }
    }

    public virtual void OnHitSpecialEffect(Base_MobStatus status)
    {

    }

    // ”\—ÍÚ×
    [Multiline(6)] public string description;
}
