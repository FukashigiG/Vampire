using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/Treasure Data/AddHSpE")]
public class AddHSpETreasure : Base_TreasureData
{
    // �������Ă���ԁA�v���C���[�̈�������̑����̃i�C�t�ɓ���\�͂�ǉ�

    public Element targetEnum;

    public BaseHSpE hspe;

    public override void OnAdd(PlayerStatus status)
    {

    }

    public override void OnRemove(PlayerStatus status)
    {

    }

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        status.attack.onThrowKnife.Subscribe(_knifeData =>
        {
            // ���̃i�C�t�f�[�^�̑������ΏۂłȂ��A�܂��͊��ɂ��̔\�͂������Ă�Ȃ疳��
            if (_knifeData.element != targetEnum) return;
            if (_knifeData.specialEffects.Contains(hspe)) return;

            // �����œn���ꂽ�i�C�t�̃f�[�^�ɁA����̓���\�͂�ǉ�
            _knifeData.specialEffects.Add(hspe);

            //Debug.Log("Add" + hspe.effectName + "for : " + _knifeData._name);
        })
        .AddTo(disposables);
    }
}
