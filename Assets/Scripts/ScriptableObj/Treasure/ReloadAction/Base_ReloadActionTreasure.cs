using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/Treasure Data/Reload/base")]
public class Base_ReloadActionTreasure : Base_TreasureData
{
    // �������Ă���ԁA�v���C���[�̃����[�h�ɔ�������

    public override void OnAdd(PlayerStatus status)
    {

    }

    public override void OnRemove(PlayerStatus status)
    {

    }

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        status.attack.onReload.Subscribe(_knives =>
        {
            ReloadAction(status, _knives);
        })
        .AddTo(disposables);
    }

    public virtual void ReloadAction(PlayerStatus status, List<KnifeData> knives)
    {

    }
}
