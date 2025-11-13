using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewKnifeAbility", menuName = "Game Data/KnifeAbility/Crush")]
public class KAL_Crush : Base_KnifeAbilityLogic
{
    // HPが一定以下のヒットした敵を即死させる

    [SerializeField] int border_Die;

    public override void ActivateAbility(Base_MobStatus status, GameObject knifeObj, KnifeData_RunTime knifeData, string effectID)
    {
        base.ActivateAbility(status, knifeObj, knifeData, effectID);

        // モデファイヤ以下なら、死亡関数を実行
        if(status.hitPoint.Value <= border_Die)　status.Die();
    }
}
