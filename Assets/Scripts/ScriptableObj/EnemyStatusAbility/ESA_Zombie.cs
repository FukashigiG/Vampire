using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewESA", menuName = "Game Data/EnemyStatusAbilityData/Zombie")]
public class ESA_Zombie : Base_EnemyStatusAbilityData
{
    // 致命ダメージ時に一回だけHP全回復する

    public override void ApplyAbility(EnemyStatus status, CompositeDisposable disposables)
    {
        bool act = true;

        status.onDamaged.Subscribe(x =>
        {
            // 現在HP以上のダメージを受けてかつ効果が未発動なら
            if(x.amount >= status.hitPoint.Value && act)
            {
                act = false;

                // HealHP関数内でHpは溢れないようになっている
                // ぶっちゃけ9999とかでもいい
                status.HealHP(status.maxHP);
            }

        }).AddTo(disposables);
    }
}
