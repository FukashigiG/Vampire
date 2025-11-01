using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/Treasure Data/BoostBlaze")]
public class BoostBlazeTreasure : Base_TreasureData
{
    // ŠŽ‚µ‚Ä‚¢‚éŠÔA‰Î‰Š‚ÌŒø‰Ê‚ð‘‹­

    [SerializeField] StatusEffectType targetEffectType = StatusEffectType.Blaze;
    [SerializeField] float amount_ExtraDuration;
    [SerializeField] int amount_;

    public override void OnAdd(PlayerStatus status)
    {

    }

    public override void OnRemove(PlayerStatus status)
    {

    }

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        Base_MobStatus.onGetStatusEffect.Subscribe(c =>
        {
            if (c.type == targetEffectType) c.duration += amount_ExtraDuration;

        }).AddTo(disposables);
    }
}
