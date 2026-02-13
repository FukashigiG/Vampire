using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System.Linq;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/TreasureLogic/Boss_Heal")]
public class TL_BossToHeal : Base_TreasureLogic
{
    // 所持している間、ボス戦開始時に回復
    // 回復量は所持ナイフ依存

    [SerializeField] int healRatio_Percent;
    [SerializeField] Element targetElement;

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        GameAdmin.Instance.onBossAppear.Subscribe(x =>
        {
            // 対象属性のナイフそれぞれの重複度の合計を取得
            int total = status.inventory.runtimeKnives
            .Where(x => x.element == targetElement)
            .Sum(x => x.count_Multiple);

            // 一本につき最大HPのN%
            status.HealHP(status.maxHP / 100 * total * healRatio_Percent);

        }).AddTo(disposables);
    }
}
