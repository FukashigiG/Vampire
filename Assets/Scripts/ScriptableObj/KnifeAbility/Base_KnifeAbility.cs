using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

//[CreateAssetMenu(fileName = "NewHSpE", menuName = "Game Data/zzzDont")]
public class Base_KnifeAbility : ScriptableObject, IOnHitSpecialEffect
{
    // このHSpEの名前
    [field: SerializeField] public string effectName { get; private set; }
    // 能力の詳細説明テキスト
    [field: SerializeField, Multiline(6)] public string description { get; private set; }

    // 効果の発動確率
    public int probability_Percent;

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

    // ナイフが投げられたときに呼ばれる
    public virtual void OnThrown(Base_MobStatus status, Vector2 posi, KnifeData_RunTime knifeData)
    {
        //Debug.Log(probability_Percent);

        // 変数がfalseなら無視
        if (!effectOnThrown) return;

        // 1～100の乱数が発動確率以内なら、特殊効果を発動
        int randomNum = Random.Range(1, 101);

        if (randomNum <= probability_Percent)
        {
            // 発動通知
            onEffectActived.OnNext(status);

            // 効果処理
            ActivateEffect_OnHit(status, posi, knifeData);
        }
    }

    // 特殊効果処理
    protected virtual void ActivateEffect_OnThrown(Base_MobStatus status, Vector2 posi, KnifeData_RunTime knifeData)
    {
        
    }


    // ナイフがヒットした時に呼ばれる
    public virtual void OnHit(Base_MobStatus status, Vector2 posi, KnifeData_RunTime knifeData)
    {
        //Debug.Log(probability_Percent);

        // 変数がfalseなら無視
        if (!effectOnHit) return;

        // 1～100の乱数が発動確率以内なら、特殊効果を発動
        int randomNum = Random.Range(1, 101);

        if (randomNum <= probability_Percent)
        {
            // 発動通知
            onEffectActived.OnNext(status);

            // 効果処理
            ActivateEffect_OnHit(status, posi, knifeData);
        }
    }

    // 特殊効果処理
    protected virtual void ActivateEffect_OnHit(Base_MobStatus status, Vector2 posi, KnifeData_RunTime knifeData)
    {

    }
}
