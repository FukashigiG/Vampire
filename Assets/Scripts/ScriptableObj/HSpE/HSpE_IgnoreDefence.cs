using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHSpE", menuName = "Game Data/HSpE/IgnoreDefence")]
public class HSpE_IgnoreDefence : BaseHSpE
{
    // IgnoreDefence‚ğtrue‚Éã‘‚«‚·‚é‚±‚Æ‚Å–hŒä–³‹‚ğ‹–‰Â
    public override bool ignoreDefence
    {
        get { return true; } 
    }

    public override void OnHitSpecialEffect(Base_MobStatus status, Vector2 p)
    {

    }
}
