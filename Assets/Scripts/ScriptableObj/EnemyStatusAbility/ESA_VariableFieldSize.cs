using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewESA", menuName = "Game Data/EnemyStatusAbilityData/VariableFieldSize")]
public class ESA_VariableFieldSize : Base_EnemyStatusAbilityData
{
    // ダメージを受けるとフィールドの大きさが変化する

    [SerializeField] float sizeChangeAmount;

    public override void ApplyAbility(EnemyStatus status, CompositeDisposable disposables)
    {
        if(status.ctrler is EnemyCtrler_Fielder fielder)
        {
            status.onDamaged.Subscribe(x =>
            {
                // 被ダメがHP以上なら発動しない
                if (x.amount > status.hitPoint.Value) return;

                // フィールドの大きさをSize change amountの分だけ変化させる
                fielder.field.SetScale(fielder.field.cullentRadius + sizeChangeAmount);

            }).AddTo(disposables);
        } 
    }
}
