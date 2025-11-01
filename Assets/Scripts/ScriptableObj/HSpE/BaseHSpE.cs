using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHSpE", menuName = "Game Data/zzzDont")]
public class BaseHSpE : ScriptableObject, IOnHitSpecialEffect
{
    [field: SerializeField] public string effectName { get; private set; }

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

    // Œø‰Ê‚ª”­“®‚µ‚½‚É’Ê’m‚ğ”ò‚Î‚·
    public static Subject<Base_MobStatus> onEffectActived = new Subject<Base_MobStatus>();

    public virtual void OnHitSpecialEffect(Base_MobStatus status, Vector2 posi, KnifeData_RunTime knifeData)
    {

    }

    // ”\—ÍÚ×
    [Multiline(6)] public string description;
}
