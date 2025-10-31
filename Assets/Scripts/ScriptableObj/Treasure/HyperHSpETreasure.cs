using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/Treasure Data/HyperHSpE")]
public class HyperHSpETreasure : Base_TreasureData
{
    // �������Ă���ԁA�v���C���[�̈����i�C�t�̓���̓���\�͂�����

    [field: SerializeField] public BaseHSpE hspe { get; private set; }

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
            if(! _knifeData.specialEffects.Contains(hspe)) return;
        })
        .AddTo(disposables);
    }
}
