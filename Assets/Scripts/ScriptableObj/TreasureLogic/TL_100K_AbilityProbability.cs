using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System.Linq;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/TreasureLogic/100KAProbability")]
public class TL_100K_AbilityProbability : Base_TreasureLogic
{
    // N秒に一度、特定の種類の能力が確定で発動する

    // 強化させたい特殊能力
    [field: SerializeField] public Base_KnifeAbilityLogic targetAbilityLogic { get; private set; }
    // ↑の型を示す
    System.Type targetType => targetAbilityLogic.GetType();

    [SerializeField] float coolDownSeconds;

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        bool isCooling = false;
        var token = status.GetCancellationTokenOnDestroy();

        status.attack.onThrowKnife.Subscribe(async _throw =>
        {
            if (isCooling) return;

            // 対象のアビリティーロジックがあれば、それを取得
            KnifeAbility matchedAbility = _throw.abilities
                .FirstOrDefault(effect => effect.abilityLogic.GetType() == targetType);

            if (matchedAbility == null) return;

            // 発生確率を上げる
            matchedAbility.abilityLogic.probability_Percent = 100;

            subject_OnAct.OnNext(Unit.Default);

            isCooling = true;

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
            isCooling = false;
        })
        .AddTo(disposables);
    }
}
