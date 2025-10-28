using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/Treasure Data/RaiseStatus")]
public class RiseStatusTreasure : Base_TreasureData
{
    // �������Ă���ԁA����̃X�e�[�^�X�����{�����㏸������

    public int ratio;

    public enum targetStatus
    {
        power, diffence, speed, luck, eyeSight
    }

    // ���ʑΏ�
    [SerializeField] targetStatus _status;

    public override void OnAdd(PlayerStatus status)
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
                status.luck *= (1 + ratio / 100);
                break;

            case targetStatus.eyeSight:
                status.eyeSight *= (1 + ratio / 100);
                break;
        }
    }

    public override void OnRemove(PlayerStatus status)
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
                status.luck /= (1 + ratio / 100);
                break;

            case targetStatus.eyeSight:
                status.eyeSight /= (1 + ratio / 100);
                break;
        }
    }

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {

    }
}
