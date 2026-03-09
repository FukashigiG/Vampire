using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UniRx;
using System.Linq;

[CreateAssetMenu(fileName = "New_PCA", menuName = "Game Data/P_CharaAbility/Slash")]
public class PCA_Slash : Base_P_CharaAbility
{
    // キャラアビリティデータ、直線状に移動し、その線上の敵全てにダメージ
    // ダメージは同じ属性のナイフの種類数がおおいほど増える

    [SerializeField] float length = 10;
    [SerializeField] int baseDamage = 30;

    [SerializeField] float dulation;
    [SerializeField] int amount_percent;
    [SerializeField] Base_StatusEffectData statusEffect;

    [SerializeField] GameObject fx;

    [SerializeField] LayerMask targetLayer;

    public override async UniTask ActivateAbility(CancellationToken token)
    {
        Vector2 targetPos;

        // 入力があればその方向、無ければ攻撃対象の方に突っ込むようにする
        if(player.ctrler.inputValue.magnitude > 0.01f)
        {
            targetPos = (Vector2)player.transform.position + player.ctrler.inputValue;
        }
        else if(player.attack.targetEnemy != null)
        {
            targetPos = player.attack.targetEnemy.transform.position;
        }
        else
        {
            targetPos = Vector2.zero;
        }

        //await UniTask.WaitUntil(() => player.attack.targetEnemy != null, cancellationToken: token);

        Vector2 dir = (targetPos - (Vector2)player.transform.position).normalized * (length);

        Vector2 center = (Vector2)player.transform.position + dir / 2;
        Vector2 size = new Vector2(4, dir.magnitude);
        float angle = Vector2.SignedAngle(Vector2.up, dir);

        //一定範囲内の敵を配列に格納
        Collider2D[] hits = Physics2D.OverlapBoxAll(center, size, angle, targetLayer);

        Vector2 afterPos = (Vector2)player.transform.position + dir;
        float areaSyze = GameAdmin.Instance.size_PlayArea;
        afterPos = new Vector2(Mathf.Clamp(afterPos.x, -areaSyze, areaSyze), Mathf.Clamp(afterPos.y, -areaSyze, areaSyze));

        player.transform.position = afterPos;

        Instantiate(fx, center, Quaternion.Euler(0, 0, angle));

        await UniTask.Delay(500);

        int total = player.inventory.runtimeKnives
            .Where(x => player.masteredElements.Contains(x.element))
            .Count();
            //.Sum(x => x.count_Multiple);

        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent(out Base_MobStatus ms))
            {
                ms.GetAttack(0, baseDamage + (int)(Mathf.Pow(total, 1.5f) * 2), dir / 2);
            }
        }

        /*
        int base_Bonus = (int)(requireChargeValue * 0.85f);
        float bonusRatio = (1f - (float)player.hitPoint.Value / (float)player.maxHP);

        player.attack.AbilityCharge((int)(base_Bonus * bonusRatio));
        */

        player.ApplyStatusEffect(statusEffect, "slashefct5423", dulation, amount_percent);

    }
}
