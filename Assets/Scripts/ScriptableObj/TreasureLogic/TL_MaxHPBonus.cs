using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/TreasureLogic/MaxHPBonus")]
public class TL_MaxHPBonus : Base_TreasureLogic
{
    // HPが最大ならバフ

    public enum statusEnum
    {
        power, diffence, speed, luck, eyeSight
    }

    // 効果対象
    [SerializeField] statusEnum targetStatusType;

    [SerializeField] int buffValue;

    bool isBonusActive = false;

    PlayerStatus status;

    public override void AddedTrigger(PlayerStatus _status)
    {
        status = _status;

        // バフを適用するかの判断
        judge();
    }

    public override void RemovedTrigger(PlayerStatus status)
    {
        // バフを解除
        if(isBonusActive) DeclineStatus();
    }

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        status.hitPoint.Subscribe(x => {

            judge();

        }).AddTo(disposables);
    }

    void judge()
    {
        bool x = status.hitPoint.Value == status.maxHP;

        if(x == true && !isBonusActive)
        {
            RiseStatus();

            isBonusActive = true;
        } 
        else if (x == false && isBonusActive)
        {
            DeclineStatus();

            isBonusActive = false;
        }
    }

    void RiseStatus()
    {
        // targetStatusによって効果を適用させるステータスを切り替える
        switch (targetStatusType)
        {
            case statusEnum.power:
                status.enhancementRate_Power += buffValue;
                break;

            case statusEnum.diffence:
                status.enhancementRate_Defence += buffValue;
                break;

            case statusEnum.speed:
                status.enhancementRate_MoveSpeed += buffValue;
                break;

            case statusEnum.luck:
                status.enhancementRate_Luck += buffValue;
                break;

            case statusEnum.eyeSight:
                status.enhancementRate_EyeSight += buffValue;
                break;
        }
    }

    void DeclineStatus()
    {
        // targetStatusによって効果を適用させるステータスを切り替える
        switch (targetStatusType)
        {
            case statusEnum.power:
                status.enhancementRate_Power -= buffValue;
                break;

            case statusEnum.diffence:
                status.enhancementRate_Defence -= buffValue;
                break;

            case statusEnum.speed:
                status.enhancementRate_MoveSpeed -= buffValue;
                break;

            case statusEnum.luck:
                status.enhancementRate_Luck -= buffValue;
                break;

            case statusEnum.eyeSight:
                status.enhancementRate_EyeSight -= buffValue;
                break;
        }
    }
}
