using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/Treasure Data/DamageToHSpE")]
public class DamageToHSpETreasure : Base_TreasureData
{
    // 所持している間、プレイヤーが攻撃を受けると次投げるナイフにHSpEを追加

    [SerializeField] BaseHSpE hspe;

    public override void OnAdd(PlayerStatus status)
    {
        
    }

    public override void OnRemove(PlayerStatus status)
    {
        
    }

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        bool standBy = false;

        PlayerStatus.onDamaged.Subscribe(x =>
        {
            standBy = true;

        }).AddTo(disposables);

        status.attack.onThrowKnife.Subscribe(KnifeData =>
        {
            if (!standBy) return;

            KnifeData.specialEffects.Add(hspe);

            standBy = false;

        }).AddTo(disposables);
    }
}
