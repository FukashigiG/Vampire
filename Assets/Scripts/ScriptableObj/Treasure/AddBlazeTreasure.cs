using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/Treasure Data/AddBlaze")]
public class AddBlazeTreasure : Base_TreasureData
{
    // �������Ă���ԁA�v���C���[�̈����Α����̃i�C�t�S�Ăɓ���\�́h�Ή��h��t�^

    public BaseHSpE blazeData;

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
            if (_knifeData.element != KnifeData.ElementEnum.red) return;
            if (_knifeData.specialEffects.Contains(blazeData)) return;

            _knifeData.specialEffects.Add(blazeData);

            Debug.Log("Set blaze for : " + _knifeData._name);
        })
        .AddTo(disposables);
    }
}
