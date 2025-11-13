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
    public override void ActivateAbility(Base_MobStatus status, GameObject knifeObj, KnifeData_RunTime knifeData, string effectID)
    {
        base.ActivateAbility(status, knifeObj, knifeData, effectID);

        _critical = true;

        Instantiate(efect, knifeObj.transform.position, Quaternion.identity);

    }
}
