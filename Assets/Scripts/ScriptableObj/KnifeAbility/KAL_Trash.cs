using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewKnifeAbility", menuName = "Game Data/KnifeAbility/Trash")]
public class KAL_Trash : Base_KnifeAbilityLogic
{
    // 投擲時、手持ちのナイフをN本捨てる

    [SerializeField] int amount_Trash;

    public override void ActivateAbility(Base_MobStatus status, GameObject knifeObject, KnifeData_RunTime knifeData, string effectID)
    {
        base.ActivateAbility(status, knifeObject, knifeData, effectID);

        if(status is PlayerStatus player)
        {
            int randomIndex;

            for(int i = 0; i < amount_Trash; i++)
            {
                if(player.attack.GetHandCount() >  0)
                {
                    randomIndex = Random.Range(0, player.attack.GetHandCount());

                    player.attack.TrashKnife(randomIndex);
                }
            }
        }
    }
}
