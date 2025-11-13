using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewKnifeAbility", menuName = "Game Data/KnifeAbility/IgnoreDefence")]
public class KAL_IgnoreDefence : Base_KnifeAbilityLogic
{
    // IgnoreDefence‚ğtrue‚Éã‘‚«‚·‚é‚±‚Æ‚Å–hŒä–³‹‚ğ‹–‰Â

    bool _ignoreDefence = false;

    public override bool ignoreDefence
    {
        get { return _ignoreDefence; } 
    }

    public override void ActivateAbility(Base_MobStatus status, GameObject knifeObj, KnifeData_RunTime knifeData, string effectID)
    {
        base.ActivateAbility(status, knifeObj, knifeData, effectID);

        _ignoreDefence = true;
    }
}
