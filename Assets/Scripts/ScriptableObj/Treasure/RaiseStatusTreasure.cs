using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/Treasure Data/RaiseStatus")]
public class RaiseStatusTreasure : Base_TreasureData
{
    // 所持している間、特定のステータスが上昇する秘宝

    public int amount;

    public enum raisedStatus
    {
        power, diffence, speed, luck, eyeSight
    }

    [SerializeField] raisedStatus _status;

    public override void OnAdd(PlayerStatus status)
    {
        switch (_status)
        {
            case raisedStatus.power:
                status.throwPower += amount;
                break;

            case raisedStatus.diffence:
                status.defence += amount;
                break;

            case raisedStatus.speed:
                status.moveSpeed += amount;
                break;

            case raisedStatus.luck:
                status.luck += amount;
                break;

            case raisedStatus.eyeSight:
                status.eyeSight += amount;
                break;
        }
    }

    public override void OnRemove(PlayerStatus status)
    {
        switch (_status)
        {
            case raisedStatus.power:
                status.throwPower -= amount;
                break;

            case raisedStatus.diffence:
                status.defence -= amount;
                break;

            case raisedStatus.speed:
                status.moveSpeed -= amount;
                break;

            case raisedStatus.luck:
                status.luck -= amount;
                break;

            case raisedStatus.eyeSight:
                status.eyeSight -= amount;
                break;
        }
    }

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {

    }
}
