using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System.Linq;
using Cysharp.Threading.Tasks;
using System;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/TreasureLogic/TimerTrigger_BlazeandBuff")]
public class TL_TimerTriggerStatusEffect : Base_TreasureLogic
{
    // 一定時間ごとに、ナイフを投げると自身が火炎状態とバフを受ける

    [SerializeField] float effectDuration;

    [SerializeField] int coolTime;

    [Serializable] class effectData
    {
        public Base_StatusEffectData statusEffect;
        public string effectID;
        public int amount;
    }

    [SerializeField] List<effectData> effects;

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        int timeCount = 0;

        status.onSecond.Subscribe(x  =>
        {
            timeCount++;

            if (timeCount >= coolTime)
            {
                timeCount = 0;

                foreach (var effect in effects)
                {
                    status.ApplyStatusEffect(effect.statusEffect, effect.effectID, effectDuration, effect.amount);
                }

                // 発動を通知
                subject_OnAct.OnNext(Unit.Default);
            }

        }).AddTo(disposables);
    }
}
