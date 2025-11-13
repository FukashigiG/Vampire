using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewKnifeAbility", menuName = "Game Data/KnifeAbility/InstantlyAddKnife")]
public class KAL_InstantlyAddKnife : Base_KnifeAbilityLogic
{
    // “Š±‘¦À‚Éè‚¿ƒiƒCƒt‚É‰½‚©‚µ‚ç‚ğ’Ç‰Á

    [SerializeField] KnifeData knifeData_Added;

    public override void ActivateAbility(Base_MobStatus status, GameObject knifeObject, KnifeData_RunTime knifeData, string effectID)
    {
        base.ActivateAbility(status, knifeObject, knifeData, effectID);

        if(status is PlayerStatus player)
        {
            player.attack.SetHand(knife: new KnifeData_RunTime(knifeData_Added));
        }
    }
}
