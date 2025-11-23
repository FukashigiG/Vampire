using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewESA", menuName = "Game Data/EnemyStatusAbilityData/Spike")]
public class ESA_Spike : Base_EnemyStatusAbilityData
{
    // ノックバックが無効になる

    public override void ApplyAbility(EnemyStatus status, CompositeDisposable disposables)
    {
        status.applied_AllowKnickBack += 1;

        // 何かしらで破棄されたら解除
        Disposable.Create(() =>
        {
            if(status != null)
            {
                status.applied_AllowKnickBack -= 1;
            }

        }).AddTo(disposables);
    }
}
