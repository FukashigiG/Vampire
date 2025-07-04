using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHSpE", menuName = "Game Data/zzzDont")]
public class BaseHSpE : ScriptableObject, IOnHitSpecialEffect
{
    public string effectName;

    public virtual bool dontDestroyBullet
    {
        get { return false; }
    }

    public virtual bool ignoreDefence
    {
        get { return false; }
    }

    public virtual bool critical
    {
        get { return false; }
        set { }
    }

    public virtual void OnHitSpecialEffect(Base_MobStatus status, Vector2 posi)
    {

    }

    // î\óÕè⁄ç◊
    [Multiline(6)] public string description;
}
