using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/TreasureLogic/AddK_Ability_For1st")]
public class TL_AddK_Ability_For1st : Base_TreasureLogic
{
    // 各初撃にアビリティ付与

    [SerializeField] KnifeAbility knifeAbility;

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        status.attack.onReload.Subscribe(hand =>
        {
            hand[0].abilities.Add(new KnifeAbility(UnityEngine.Object.Instantiate(knifeAbility.abilityLogic), knifeAbility.effectID));

        }).AddTo(disposables);
    }
}
