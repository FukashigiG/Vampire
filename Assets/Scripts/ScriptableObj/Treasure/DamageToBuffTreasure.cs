using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/Treasure Data/DamageToBuff")]
public class DamageToBuffTreasure : Base_TreasureData
{
    // 所持している間、プレイヤーが攻撃を受けるとバフが入る

    [SerializeField] StatusEffectType effectType;
    [SerializeField] string effectID;
    [SerializeField] float duration;
    [SerializeField] int amount;

    public override void OnAdd(PlayerStatus status)
    {
        
    }

    public override void OnRemove(PlayerStatus status)
    {
        
    }

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        PlayerStatus.onDamaged.Subscribe(x =>
        {
            status.ApplyStatusEffect(effectType, effectID, duration, amount);

        }).AddTo(disposables);
    }
}
