using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

//[CreateAssetMenu(fileName = "NewHSpE", menuName = "Game Data/zzzDont")]
public class Base_KnifeAbilityLogic : ScriptableObject, IOnHitSpecialEffect
{
    // このHSpEの名前
    [field: SerializeField] public string effectName { get; private set; }
    // 能力の詳細説明テキスト
    [field: SerializeField, Multiline(6)] public string description { get; private set; }

    // 投げられたとき、ヒットした時それぞれ効果が発動するか
    [field: SerializeField] public bool effectOnThrown { get; private set; }
    [field: SerializeField] public bool effectOnHit { get; private set; }

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

    // 効果が発動した時に通知を飛ばす
    public static Subject<Base_MobStatus> onEffectActived = new Subject<Base_MobStatus>();
    

    // 特殊効果処理
    public virtual void ActivateAbility(Base_MobStatus status, GameObject knifeObject, KnifeData_RunTime knifeData, float modifire, string effectID)
    {


        // 発動通知
        onEffectActived.OnNext(status);
    }
}
