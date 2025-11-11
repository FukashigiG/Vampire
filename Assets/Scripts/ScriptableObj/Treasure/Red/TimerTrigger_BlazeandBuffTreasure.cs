using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System.Linq;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/Treasure Data/TimerTrigger_BlazeandBuff")]
public class TimerTrigger_BlazeandBuffTreasure : Base_TreasureData
{
    // 一定時間ごとに、ナイフを投げると自身が火炎状態とバフを受ける

    [SerializeField] float coolDownSeconds;

    [SerializeField] float effectDuration;

    [SerializeField] string blazeEffectID;

    [SerializeField] Base_StatusEffectData statusEffect_1;
    [SerializeField] string effectID_1;
    [SerializeField] int amount1;

    [SerializeField] Base_StatusEffectData statusEffect_2;
    [SerializeField] string effectID_2;
    [SerializeField] int amount2;

    public override void OnAdd(PlayerStatus status)
    {

    }

    public override void OnRemove(PlayerStatus status)
    {

    }

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        bool isCooling = false;
        var token = status.GetCancellationTokenOnDestroy();

        status.attack.onThrowKnife.Subscribe(async x =>
        {
            // クールタイム中なら無視
            if (isCooling) return;

            isCooling = true;

            status.ApplyStatusEffect(statusEffect_2, effectID_2, effectDuration, amount2);
            status.ApplyStatusEffect(statusEffect_2, effectID_2, effectDuration, amount2);

            // 待つ
            try
            {
                await UniTask.Delay((int)(coolDownSeconds * 1000), cancellationToken: token);
            }
            catch (System.OperationCanceledException)
            {
                return;
            }

            // クールタイム解除
            isCooling = false ;

        }).AddTo(disposables);
    }
}
