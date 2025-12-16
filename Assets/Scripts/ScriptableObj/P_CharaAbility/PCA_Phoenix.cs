using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.XR;

[CreateAssetMenu(fileName = "New_PCA", menuName = "Game Data/P_CharaAbility/Phoenix")]
public class PCA_Phoenix : Base_P_CharaAbility
{
    // キャラアビリティのテスト用データ、エフェクトを生成する

    [SerializeField, Range(1, 100)] int ratio_Heal_Parcent;

    async public override UniTask ActivateAbility(CancellationToken token)
    {
        player.HealHP((int)((float)player.maxHP * (float)ratio_Heal_Parcent / 100f));

        // 重複度が一番高いナイフを[初期化された状態で]取得
        var knife = new KnifeData_RunTime(player.inventory.runtimeKnives.OrderByDescending(x => x.count_Multiple).FirstOrDefault());

        Vector2 target = Vector2.zero;

        if(player.attack.targetEnemy != null)
        {
            target = player.attack.targetEnemy.transform.position;
        }
        else
        {
            target = player.transform.up;
        }

        // 攻撃対象の方向をVec2型で取得
        Vector2 dir = (target - (Vector2)player.transform.position).normalized;

        // それをQuaternionに変換
        Quaternion baseRotation = Quaternion.FromToRotation(Vector2.up, dir);

        player.enhancementRate_Power += 50;

        try
        {
            for (int q = 0; q < 10; q++)
            {
                Quaternion rotationOffset = Quaternion.Euler(0, 0, 360 / 10 * q);

                // ベースの方向と合成
                Quaternion finalRotation = baseRotation * rotationOffset;

                player.attack.ThrowKnife(knife, finalRotation);

                // 一瞬待機
                await UniTask.Delay((int)(70), cancellationToken: token);
            }

            await UniTask.Delay((int)(600), cancellationToken: token);
        }
        finally
        {
            player.enhancementRate_Power -= 50;
        }
    }
}
