using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/Treasure Data/BoostStatusEffect")]
public class BoostStatusEffectTreasure : Base_TreasureData
{
    // ŠŽ‚µ‚Ä‚¢‚éŠÔA“G‚©Ž©•ª‚ªŽó‚¯‚éó‘Ô•Ï‰»‚ÌŒø‰ÊŽžŠÔ‚ð‘Œ¸

    // ‘ÎÛ‚Í“G‚©Ž©•ª‚©
    [SerializeField] bool isTargetEnemy;
    // ‘ÎÛ‚Ìó‘Ô•Ï‰»
    [SerializeField] StatusEffectType targetEffectType;
    // ‘Œ¸‚³‚¹‚éŒø‰ÊŽžŠÔ—Ê
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
        switch (isTargetEnemy)
        {
            case true:

                EnemyStatus.onGetStatusEffect.Subscribe(c =>
                {
                    if (c.type == targetEffectType) c.duration += amount_ExtraDuration;

                }).AddTo(disposables);

                break;

            case false:

                PlayerStatus.onGetStatusEffect.Subscribe(c =>
                {
                    if (c.type == targetEffectType) c.duration += amount_ExtraDuration;

                }).AddTo(disposables);

                break;
        }


    }
}
