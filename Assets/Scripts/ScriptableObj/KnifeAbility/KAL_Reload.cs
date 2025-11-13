using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewKnifeAbility", menuName = "Game Data/KnifeAbility/Reload")]
public class KAL_Reload : Base_KnifeAbilityLogic
{
    // 発動時、強制的に再リロードさせる

    public override void ActivateAbility(Base_MobStatus status, GameObject knifeObject, KnifeData_RunTime knifeData, string effectID)
    {
        base.ActivateAbility(status, knifeObject, knifeData,effectID);

        if(status is PlayerStatus player)
        {
            player.attack.StartAttakLoop();
        }
    }
}
