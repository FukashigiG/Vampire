using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/Treasure Data/ReferKnifeCount")]
public class ReferKnifeCountTreasure : Base_TreasureData
{
    // �v���C���[�̏�������i�C�t�̐����Q�Ƃ��ăX�e�[�^�X���㏸��������

    public enum statusEnum
    {
        power, diffence, speed, luck, eyeSight
    }

    // ���ʑΏ�
    [SerializeField] statusEnum targetStatus;

    // �i�C�t1�{������̌��ʊ���
    public float xBonusRatio;

    // ���݂̌��ʗ�
    float currentBonusValue;

    public override void OnAdd(PlayerStatus status)
    {
        // �{�[�i�X�l���v�Z
        currentBonusValue = status.inventory.runtimeKnives.Count * xBonusRatio;

        // �o�t��K�p
        RiseStatus(currentBonusValue, status);
    }

    public override void OnRemove(PlayerStatus status)
    {
        // ���݂̃o�t������
        DeclineStatus(currentBonusValue, status);
    }

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        // runtimeKnives�̗v�f�����ς�邽�тɌĂ΂��
        status.inventory.runtimeKnives.ObserveCountChanged().Subscribe(newCount =>
        {
            RecalculateBonus(newCount, status);
        })
        .AddTo(disposables);
    }

    // �{�[�i�X���Čv�Z���ēK�p���郁�\�b�h
    private void RecalculateBonus(int x, PlayerStatus _status)
    {
        // 1. �܂����݂̃o�t����x��菜��
        DeclineStatus(currentBonusValue, _status);

        // 2. �V�����{�[�i�X���v�Z����
        float newBonus = x * xBonusRatio;

        // 3. �V�����o�t��K�p����
        RiseStatus(newBonus, _status);

        // 4. ���݂̃{�[�i�X�l���X�V����
        currentBonusValue = newBonus;
    }

    void RiseStatus(float ratio, PlayerStatus status)
    {
        // targetStatus�ɂ���Č��ʂ�K�p������X�e�[�^�X��؂�ւ���
        switch (targetStatus)
        {
            case statusEnum.power:
                status.power *= (1 + ratio);
                break;

            case statusEnum.diffence:
                status.defence *= (1 + ratio);
                break;

            case statusEnum.speed:
                status.moveSpeed *= (1 + ratio);
                break;

            case statusEnum.luck:
                status.luck *= (1 + ratio);
                break;

            case statusEnum.eyeSight:
                status.eyeSight *= (1 + ratio);
                break;
        }
    }

    void DeclineStatus(float ratio, PlayerStatus status)
    {
        // targetStatus�ɂ���Č��ʂ�K�p������X�e�[�^�X��؂�ւ���
        switch (targetStatus)
        {
            case statusEnum.power:
                status.power /= (1 + ratio);
                break;

            case statusEnum.diffence:
                status.defence /= (1 + ratio);
                break;

            case statusEnum.speed:
                status.moveSpeed /= (1 + ratio);
                break;

            case statusEnum.luck:
                status.luck /= (1 + ratio);
                break;

            case statusEnum.eyeSight:
                status.eyeSight /= (1 + ratio);
                break;
        }
    }

    
}
