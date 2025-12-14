using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewESA", menuName = "Game Data/EnemyStatusAbilityData/DamageToBuff")]
public class ESA_DamageToBuff : Base_EnemyStatusAbilityData
{
    // ダメージを受けるたびにステータスが上がる

    [Serializable] enum EffectType { atk, def, spd }

    [SerializeField] EffectType effectType = EffectType.spd;

    [SerializeField] int eachBonusAmount;


    public override void ApplyAbility(EnemyStatus status, CompositeDisposable disposables)
    {
        //int cullentBonusAmount = 0;

        status.onDamaged.Subscribe(x =>
        {
            switch (effectType)
            {
                case EffectType.atk:
                    status.enhancementRate_Power += eachBonusAmount;
                    break;

                case EffectType.def:
                    status.enhancementRate_Defence += eachBonusAmount;
                    break;

                case EffectType.spd:
                    status.enhancementRate_MoveSpeed += eachBonusAmount;
                    break;
            }

        }).AddTo(disposables);
    }
}
