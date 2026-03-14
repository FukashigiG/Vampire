using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;
using System;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/TreasureLogic/SpendTurretToAddKAL")]
public class TL_SpendTurretToAddKAL : Base_TreasureLogic
{
    // リロード時、周囲の召喚物を消費して引いたナイフ全てにアビリティを付与

    [SerializeField] KnifeAbility knifeAbility;
    [SerializeField] LayerMask propsLayer;

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        status.attack.onReload.Subscribe(x =>
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(status.transform.position, status.eyeSight, propsLayer);

            if (hits.Length <= 0) return;

            Destroy(hits[0].gameObject);

            foreach (var knife in status.attack.hand_RC)
            {
                knife.abilities.Add(new KnifeAbility(UnityEngine.Object.Instantiate(knifeAbility.abilityLogic), knifeAbility.effectID));
            }

            subject_OnAct.OnNext(Unit.Default);

        }).AddTo(disposables);
    }
}
