using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/TreasureLogic/SummonObj/Damage")]
public class TL_SummonObj_Damage : Base_TreasureLogic
{
    // 所持している間、プレイヤーが攻撃を受けると精霊を生成する
    // 別に精霊じゃなくてもいいか

    // 精霊のプレハブ
    [SerializeField] GameObject summonObject;

    // 効果発動確率
    [SerializeField, Range(1, 100)] int activationProbability_Percent;

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        status.onDamaged.Subscribe(x =>
        {
            // 発動抽選
            int random = Random.Range(1, 101);
            if (random < activationProbability_Percent) return;

            // オブジェクト生成
            Instantiate(summonObject, status.transform.position, Quaternion.identity);

            subject_OnAct.OnNext(Unit.Default);

        }).AddTo(disposables);
    }
}
