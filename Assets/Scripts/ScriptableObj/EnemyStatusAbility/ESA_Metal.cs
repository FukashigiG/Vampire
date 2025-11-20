using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewESA", menuName = "Game Data/EnemyStatusAbilityData/Metal")]
public class ESA_Metal : Base_EnemyStatusAbilityData
{
    // 全ダメージが１になるアビリティ

    public override void ApplyAbility(EnemyStatus status, CompositeDisposable disposables)
    {
        // 入力された数値を問答無用で１に固定するフィルタ
        System.Func<int, int> metalFilter = (originalDamage) => 1;

        status.damageFilters.Add(metalFilter);

        // 何かしらで破棄されたら解除
        Disposable.Create(() =>
        {
            if(status != null)
            {
                status.damageFilters.Remove(metalFilter);
            }

        }).AddTo(disposables);
    }
}
