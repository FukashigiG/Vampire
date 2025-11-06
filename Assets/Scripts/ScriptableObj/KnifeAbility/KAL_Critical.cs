using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewKnifeAbility", menuName = "Game Data/KnifeAbility/Critical")]
public class KAL_Critical : Base_KnifeAbilityLogic
{
    // まれに大きなダメージ

    private bool _critical = false; // プロパティの値を保持するためのプライベートフィールド
    // これを下のcriticalに噛ませないと無限ループのエラーになる

    public override bool critical
    {
        get { return _critical; }
    }

    [SerializeField] GameObject efect;

    // 効果が発動して初めてtrueになる
    public override void ActivateEffect_OnHit(Base_MobStatus status, Vector2 posi, KnifeData_RunTime knifeData, float modifire)
    {
        base.ActivateEffect_OnHit (status, posi, knifeData, modifire);

        _critical = true;

        Instantiate(efect, posi, Quaternion.identity);

    }
}
