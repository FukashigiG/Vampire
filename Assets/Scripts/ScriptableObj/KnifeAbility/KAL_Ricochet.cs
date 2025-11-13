using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewKnifeAbility", menuName = "Game Data/KnifeAbility/Ricochet")]
public class KAL_Ricochet : Base_KnifeAbilityLogic
{
    // ’µ’e

    // dontDestroyBullet‚ğtrue‚Éã‘‚«‚·‚é‚±‚Æ‚Å’e‚ğÁ‚¦‚È‚¢‚æ‚¤‚É
    bool _dontDestroyBullet = false;

    public override bool dontDestroyBullet
    {
        get { return _dontDestroyBullet; }
    }


    public override void ActivateAbility(Base_MobStatus status, GameObject knifeObj, KnifeData_RunTime knifeData, string effectID)
    {
        base.ActivateAbility (status, knifeObj, knifeData, effectID);

        _dontDestroyBullet = true;

        knifeObj.transform.rotation = Quaternion.Euler (0, 0, Random.Range(0, 360f));
    }
}
