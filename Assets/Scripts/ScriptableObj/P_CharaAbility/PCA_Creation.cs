using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[CreateAssetMenu(fileName = "New_PCA", menuName = "Game Data/P_CharaAbility/Creation")]
public class PCA_Creation : Base_P_CharaAbility
{
    // キャラアビリティのテスト用データ、エフェクトを生成する

    [SerializeField] LayerMask propsLayer;
    [SerializeField] GameObject fx_Hit;
    [SerializeField] GameObject fx_;

    public override async UniTask ActivateAbility(CancellationToken token)
    {
        int treasureCount = player.inventory.runtimeTreasure.Count;

        Vector2 center = player.transform.position;

        Collider2D[] hits = Physics2D.OverlapCircleAll(center, 99, propsLayer);

        foreach (Collider2D hit in hits)
        {
            if(hit == null) continue;

            if (hit.TryGetComponent(out TurretCtrler ctrler))
            {
                ctrler.bulletNum += 20 + (int)(treasureCount * 2.5f);
                ctrler.eyeSight *= 3f;
                ctrler.interval_Shot_Sec *= (0.42f / (1 + treasureCount * 0.1f));
                ctrler.power += treasureCount / 2;
                ctrler.bulletSpeed *= 2.2f;

                Instantiate(fx_Hit, hit.transform.position, Quaternion.identity);
                Instantiate(fx_, hit.transform.position, Quaternion.identity, hit.transform);

                await UniTask.Delay(100, cancellationToken: token);
            }
        }

        await UniTask.Delay(1000, cancellationToken:token);
    }
}
