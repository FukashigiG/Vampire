using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.XR;

[CreateAssetMenu(fileName = "New_PCA", menuName = "Game Data/P_CharaAbility/BigMagic")]
public class PCA_BigMagic : Base_P_CharaAbility
{
    // キャラアビリティのデータ
    // 複数体召喚し、周囲の敵全てにダメージと凍結効果

    [SerializeField] int num_SummonSpilit;
    [SerializeField] int damagePoint;

    [SerializeField] GameObject spirit_Prefab;

    [SerializeField] Base_StatusEffectData statusEffect;

    [SerializeField] LayerMask spiritLayer;
    [SerializeField] LayerMask enemyLayer;

    [SerializeField] GameObject fx_Charge;
    [SerializeField] GameObject fx_Impact;

    async public override UniTask ActivateAbility(CancellationToken token)
    {
        Vector2 playerPos = PlayerController.Instance.transform.position;

        for (int i = 0;i < num_SummonSpilit; i++)
        {
            Instantiate(spirit_Prefab, playerPos + new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f)), Quaternion.identity);
        }

        Instantiate(fx_Charge, playerPos, Quaternion.identity, PlayerController.Instance.transform);

        await UniTask.Delay((int)(1000), cancellationToken: token);

        playerPos = PlayerController.Instance.transform.position;

        Instantiate(fx_Impact, playerPos, Quaternion.identity);

        Collider2D[] spirits = Physics2D.OverlapCircleAll(playerPos, 3, spiritLayer);

        float radius = spirits.Length * 2.5f;

        Collider2D[] hits = Physics2D.OverlapCircleAll(playerPos, radius, enemyLayer);

        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent(out Base_MobStatus ms))
            {
                ms.GetAttack(0, damagePoint, playerPos);

                ms.ApplyStatusEffect(statusEffect, "BigMagicEffect", spirits.Length * 0.4f, 0);
            }
        }
    }
}
