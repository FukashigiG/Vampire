using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHSpE", menuName = "Game Data/HSpE/penetration")]
public class HSpE_Penetration : BaseHSpE
{
    // dontDestroyBullet‚ğtrue‚Éã‘‚«‚·‚é‚±‚Æ‚ÅŠÑ’Ê‚ğ‹–‰Â

    bool _dontDestroyBullet = false;

    public override bool dontDestroyBullet
    {
        get { return _dontDestroyBullet; }
    }

    protected override void ActivateEffect(Base_MobStatus status, Vector2 posi, KnifeData_RunTime knifeData)
    {
        _dontDestroyBullet=true;

        base.ActivateEffect(status, posi, knifeData);
    }
}
