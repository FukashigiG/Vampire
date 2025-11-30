using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/TreasureLogic/BoostStatusEffect")]
public class TL_BoostStatusEffect : Base_TreasureLogic
{
    // ŠŽ‚µ‚Ä‚¢‚éŠÔA“G‚©Ž©•ª‚ªŽó‚¯‚éó‘Ô•Ï‰»‚ÌŒø‰ÊŽžŠÔ‚ð‘Œ¸

    // ‘ÎÛ‚Í“G‚©Ž©•ª‚©
    [SerializeField] bool isTargetEnemy;
    // ‘ÎÛ‚Ìó‘Ô•Ï‰»
    [SerializeField] Base_StatusEffectData targetEffect;
    // ‘Œ¸‚³‚¹‚éŒø‰ÊŽžŠÔ—Ê
    [SerializeField] float amount_ExtraDuration;
    [SerializeField] int amount_;

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        switch (isTargetEnemy)
        {
            case true:

                EnemyStatus.onGetStatusEffect.Subscribe(c =>
                {
                    if (c.effect == targetEffect)
                    {
                        c.duration += amount_ExtraDuration;

                        subject_OnAct.OnNext(Unit.Default);
                    }

                }).AddTo(disposables);

                break;

            case false:

                status.onGetStatusEffect.Subscribe(c =>
                {
                    if (c.statusEffect == targetEffect)
                    {
                        c.duration += amount_ExtraDuration;

                        subject_OnAct.OnNext(Unit.Default);
                    }

                }).AddTo(disposables);

                break;
        }


    }
}
