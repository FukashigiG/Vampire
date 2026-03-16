using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewESA", menuName = "Game Data/EnemyStatusAbilityData/Warp")]
public class ESA_Warp : Base_EnemyStatusAbilityData
{
    // 攻撃を受けると後方にワープする

    [SerializeField] bool warpForBack = true;

    [SerializeField] float distance;

    [SerializeField] float coolTime = 0.5f;

    [SerializeField] GameObject fx_Warp;

    public override void ApplyAbility(EnemyStatus status, CompositeDisposable disposables)
    {
        status.onDamaged
            .Where(x => x.amount <= status.hitPoint.Value)
            .ThrottleFirst(TimeSpan.FromSeconds(coolTime)) // これの意味：一度イベントを通したらN秒間同イベントを無視
            .Subscribe(x =>
        {

            Vector2 playerPosi = status.ctrler.target.transform.position;

            Vector2 dir = (playerPosi - (Vector2)status.transform.position).normalized;

            switch (warpForBack)
            {
                case true:
                    status.transform.Translate(dir * distance * -1f);
                    break;

                case false:
                    status.transform.Translate(dir * distance);
                    break;
            }

            // 移動場所にエフェクト生成
            Instantiate(fx_Warp, status.transform.position, Quaternion.identity);

        }).AddTo(disposables);
    }
}
