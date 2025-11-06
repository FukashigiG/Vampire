using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewKnifeAbility", menuName = "Game Data/KnifeAbility/penetration")]
public class KAL_Penetration : Base_KnifeAbilityLogic
{
    // dontDestroyBullet‚ğtrue‚Éã‘‚«‚·‚é‚±‚Æ‚ÅŠÑ’Ê‚ğ‹–‰Â

    bool _dontDestroyBullet = false;

    public override bool dontDestroyBullet
    {
        get { return _dontDestroyBullet; }
    }

    public override void ActivateAbility(Base_MobStatus status, GameObject knifeObj, KnifeData_RunTime knifeData, float modifire, string effectID)
    {
        base.ActivateAbility(status, knifeObj, knifeData, modifire, effectID);

        _dontDestroyBullet=true;
    }
}
