using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHSpE", menuName = "Game Data/HSpE/IgnoreDefence")]
public class HSpE_IgnoreDefence : BaseHSpE
{
    // IgnoreDefence‚ğtrue‚Éã‘‚«‚·‚é‚±‚Æ‚Å–hŒä–³‹‚ğ‹–‰Â

    bool _ignoreDefence = false;

    public override bool ignoreDefence
    {
        get { return _ignoreDefence; } 
    }

    protected override void ActivateEffect(Base_MobStatus status, Vector2 posi, KnifeData_RunTime knifeData)
    {
        _ignoreDefence = true;

        base.ActivateEffect(status, posi, knifeData);
    }
}
