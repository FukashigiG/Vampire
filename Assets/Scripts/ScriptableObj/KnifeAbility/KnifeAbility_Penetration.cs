using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewKnifeAbility", menuName = "Game Data/KnifeAbility/penetration")]
public class KnifeAbility_Penetration : Base_KnifeAbility
{
    // dontDestroyBullet‚ğtrue‚Éã‘‚«‚·‚é‚±‚Æ‚ÅŠÑ’Ê‚ğ‹–‰Â

    bool _dontDestroyBullet = false;

    public override bool dontDestroyBullet
    {
        get { return _dontDestroyBullet; }
    }

    protected override void ActivateEffect_OnHit(Base_MobStatus status, Vector2 posi, KnifeData_RunTime knifeData)
    {
        _dontDestroyBullet=true;

    }
}
