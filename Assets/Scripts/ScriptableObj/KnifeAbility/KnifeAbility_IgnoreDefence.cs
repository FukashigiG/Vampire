using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewKnifeAbility", menuName = "Game Data/KnifeAbility/IgnoreDefence")]
public class KnifeAbility_IgnoreDefence : Base_KnifeAbility
{
    // IgnoreDefence‚ğtrue‚Éã‘‚«‚·‚é‚±‚Æ‚Å–hŒä–³‹‚ğ‹–‰Â

    bool _ignoreDefence = false;

    public override bool ignoreDefence
    {
        get { return _ignoreDefence; } 
    }

    protected override void ActivateEffect_OnHit(Base_MobStatus status, Vector2 posi, KnifeData_RunTime knifeData)
    {
        _ignoreDefence = true;

    }
}
