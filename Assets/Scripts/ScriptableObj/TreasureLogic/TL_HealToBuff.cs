using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;
using System;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/TreasureLogic/HealToBuff")]
public class TL_HealToBuff : Base_TreasureLogic
{
    // HPを回復するとバフ

    
    [SerializeField] float duration;
    [SerializeField] int amount;

    [Serializable] class StatusEffects
    {
        public Base_StatusEffectData statusEffect;
        public string effectid;
    }

    [SerializeField] StatusEffects[] statusEffects;

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        status.onHeal.Subscribe(x =>
        {
            foreach(var effect in statusEffects)
            {
                status.ApplyStatusEffect(effect.statusEffect, effect.effectid, duration, amount);
            }

            subject_OnAct.OnNext(Unit.Default);

        }).AddTo(disposables);
    }
}
