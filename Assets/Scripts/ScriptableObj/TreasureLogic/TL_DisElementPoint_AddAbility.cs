using UniRx;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/TreasureLogic/DisEP_AbilityAdd")]
public class TL_DisElementPoint_AddAbility : Base_TreasureLogic
{
    // 各一投目のナイフの属性値を犠牲にアビリティを付与

    [SerializeField] int cut_EP = 14;

    [SerializeField] KnifeAbility ability;

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        bool act = true;

        status.attack.onThrowKnife.Subscribe(_throw =>
        {
            if(! act) return;

            act = false;

            _throw.elementPower -= cut_EP;
            if(_throw.elementPower < 0) _throw.elementPower = 0;

            // 対象のアビリティーロジックがあれば、それを取得
            KnifeAbility matchedAbility = _throw.abilities
                .FirstOrDefault(effect => effect.abilityLogic.effectName == ability.abilityLogic.effectName);

            // そのナイフデータの属性が対象でないなら無視、または既にこの能力を持ってるなら無視
            if (matchedAbility != null) return;

            // 引数で渡されたナイフのデータに、特定の特殊能力を追加
            _throw.abilities.Add(ability);

            subject_OnAct.OnNext(Unit.Default);

        }).AddTo(disposables);

        status.attack.onReload.Subscribe(aa =>
        {
            act = true; 

        }).AddTo(disposables);
    }
}
