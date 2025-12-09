using UnityEngine;

[CreateAssetMenu(fileName = "NewKnifeAbility", menuName = "Game Data/KnifeAbility/SummonObj")]
public class KAL_SummonObject : Base_KnifeAbilityLogic
{
    // 投擲時もしくはヒット時、何かしらのオブジェクトを付近に生成する

    [SerializeField] GameObject SummonedObject;

    public override void ActivateAbility(Base_MobStatus status, GameObject knifeObject, KnifeData_RunTime knifeData, string effectID)
    {
        base.ActivateAbility(status, knifeObject, knifeData, effectID);

        var spawnPosi = (Vector2)status.transform.position + new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f)); 

        Instantiate(SummonedObject, spawnPosi, Quaternion.identity);
    }
}
