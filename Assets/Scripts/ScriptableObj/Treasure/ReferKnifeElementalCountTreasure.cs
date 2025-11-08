using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/Treasure Data/ReferKnifeCountElemental")]
public class ReferKnifeElementalCountTreasure : Base_TreasureData
{
    // プレイヤーの所持するN属性のナイフの数がM以上なら、リロード上限をL増やす

    // 効果対象
    [SerializeField] Element targetElement;

    // 属性ナイフが何本あればいいか
    public int border;

    // 上げる対象のステータス
    [SerializeField] PlayerStatus.PlayerStatusType targetStatus_Bonus;

    // 効果量
    [SerializeField] int bonusValue;

    // 効果適用済みを判定
    bool isBonusActive = false;

    public override void OnAdd(PlayerStatus status)
    {
        // 欲しい属性値のやつが何個あるか計算
        int d = status.inventory.runtimeKnives.Count(x => x.element == targetElement);

        if(d >= border && ! isBonusActive)
        {
            status.limit_DrawKnife += bonusValue;

            isBonusActive = true;
        } 
    }

    public override void OnRemove(PlayerStatus status)
    {
        if(isBonusActive) status.limit_DrawKnife -= bonusValue;
    }

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        // runtimeKnivesの要素数が変わるたびに呼ばれる
        DetermineBonus(status);
    }
    
    // 判定
    void DetermineBonus(PlayerStatus status)
    {
        // 欲しい属性値のやつが何個あるか計算
        int d = status.inventory.runtimeKnives.Count(x => x.element == targetElement);

        // 要求値以上あり、かつまだボーナス未適用なら
        if (d >= border && !isBonusActive)
        {
            status.Modify_statusPoint(targetStatus_Bonus, bonusValue);

            isBonusActive = true;
        }
        // 要求値未満かつボーナス適用済みなら
        else if (d <= border && isBonusActive)
        {
            status.Modify_statusPoint(targetStatus_Bonus, bonusValue * -1);

            isBonusActive = false;
        }
    }
}
