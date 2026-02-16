using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System.Linq;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/TreasureLogic/SpiritLast/Bomb")]
public class TL_SpiritLastBomb : Base_TreasureLogic
{
    // 所持している間、精霊が寿命を迎えた際、その周囲の敵にダメージ

    [SerializeField] LayerMask targetLayer;
    [SerializeField] float radius = 2.4f;
    [SerializeField] float damageMultiple = 2f;

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        SpiritCtrler.onDestroy.Subscribe(spirit =>
        {
            // 死んだ精霊の位置を中心地とする
            Vector2 center = spirit.transform.position;

            // 周囲の敵を一括で取得
            Collider2D[] hits = Physics2D.OverlapCircleAll(center, radius, targetLayer);

            // それぞれに対して
            foreach (Collider2D hit in hits)
            {
                if (hit.TryGetComponent(out Base_MobStatus ms))
                {
                    // 精霊の攻撃力に倍率をかけた数値の属性ダメージ
                    ms.GetAttack(0, (int)(spirit.power * damageMultiple), center);
                }
            }

        }).AddTo(disposables);
    }
}
