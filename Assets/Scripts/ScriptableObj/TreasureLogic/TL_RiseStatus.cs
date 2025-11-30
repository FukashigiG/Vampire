using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/TreasureLogic/RaiseStatus")]
public class TL_RiseStatus : Base_TreasureLogic
{
    // 所持している間、特定のステータス強化倍率が上昇する秘宝

    public int ratio;

    public enum targetStatus
    {
        power, diffence, speed, luck, eyeSight
    }

    // 効果対象
    [SerializeField] targetStatus _status;

    public override void AddedTrigger(PlayerStatus status)
    {
        switch (_status)
        {
            case targetStatus.power:
                status.enhancementRate_Power += ratio;
                break;

            case targetStatus.diffence:
                status.enhancementRate_Defence += ratio;
                break;

            case targetStatus.speed:
                status.enhancementRate_MoveSpeed += ratio;
                break;

            case targetStatus.luck:
                status.enhancementRate_Luck += ratio;
                break;

            case targetStatus.eyeSight:
                status.enhancementRate_EyeSight += ratio;
                break;
        }
    }

    public override void RemovedTrigger(PlayerStatus status)
    {
        switch (_status)
        {
            case targetStatus.power:
                status.enhancementRate_Power -= ratio;
                break;

            case targetStatus.diffence:
                status.enhancementRate_Defence -= ratio;
                break;

            case targetStatus.speed:
                status.enhancementRate_MoveSpeed -= ratio;
                break;

            case targetStatus.luck:
                status.enhancementRate_Luck -= ratio;
                break;

            case targetStatus.eyeSight:
                status.enhancementRate_EyeSight -= ratio;
                break;
        }
    }

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {

    }
}
