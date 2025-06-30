using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHSpE", menuName = "Game Data/HSpE/penetration")]
public class HSpE_Penetration : BaseHSpE
{
    // dontDestroyBullet‚ğtrue‚Éã‘‚«‚·‚é‚±‚Æ‚ÅŠÑ’Ê‚ğ‹–‰Â
    public override bool dontDestroyBullet
    {
        get { return true; }
    }

    public override void OnHitSpecialEffect(Base_MobStatus status)
    {

    }
}
