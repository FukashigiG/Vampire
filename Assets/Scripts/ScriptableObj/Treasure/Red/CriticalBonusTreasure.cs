using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/Treasure Data/CriticalBonus")]
public class CriticalBonusTreasure : Base_TreasureData
{
    // �������Ă���ԁA�N���e�B�J����H������G�ɒǉ����ʂ�t�^

    [SerializeField] StatusEffectType effectType;
    [SerializeField] string effectID;
    [SerializeField] float duration;
    [SerializeField] int amount_Debuff;

    public override void OnAdd(PlayerStatus status)
    {

    }

    public override void OnRemove(PlayerStatus status)
    {

    }

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        // �N���e�B�J�������̔������w��
        HSpE_Critical.onEffectActived.Subscribe(targetStatus  =>
        {
            targetStatus.ApplyStatusEffect(effectType, effectID, duration ,-1 * amount_Debuff);

        }).AddTo(disposables);
    }
}
