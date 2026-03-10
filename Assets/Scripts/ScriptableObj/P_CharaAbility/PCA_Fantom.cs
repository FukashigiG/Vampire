using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[CreateAssetMenu(fileName = "New_PCA", menuName = "Game Data/P_CharaAbility/Fantom")]
public class PCA_Fantom : Base_P_CharaAbility
{
    // キャラアビリティのテスト用データ、エフェクトを生成する

    [SerializeField] int instantBuffAmount;

    [SerializeField] GameObject fx_Trail;
    [SerializeField] GameObject fx_Warp;

    public async override UniTask ActivateAbility(CancellationToken token)
    {
        //Instantiate(fx, player.transform.position, Quaternion.identity);

        // これをしておかないとトレイルエフェクトが正しく生成されない
        // 移動量で生成するタイプのパーティクルはダイナミックなRigidBodyがあるならそれのヴェロシティを参照したがるっぽい
        player.ctrler._rigidbody.bodyType = RigidbodyType2D.Kinematic;

        player.count_PermissionHit.Value++;
        player.enhancementRate_Power += instantBuffAmount;
        player.count_Actable++;

        var basePoint = player.transform.position;
        Vector2 targetPoint;

        // 攻撃目標地点について、playerattackのtargetがいればその地点
        if ((player.attack.targetEnemy != null))
        {
            targetPoint = player.attack.targetEnemy.transform.position;
        } 
        else
        {
            targetPoint = basePoint;
        }

        GameObject _fx_Trail = Instantiate(fx_Trail, player.transform.position, Quaternion.identity, player.transform);

        for (int i = 0; i < 10; i++)
        {
            // 目標地点から半径７の円周上を決定
            var randomPoint = Random.insideUnitCircle.normalized * 7 + (Vector2)targetPoint;

            player.transform.position = randomPoint;

            Instantiate(fx_Warp, randomPoint, Quaternion.identity);

            // 方向を取得
            var dir = (targetPoint - randomPoint).normalized;

            // ランダムなナイフを取得
            var randomKnife = new KnifeData_RunTime(player.inventory.runtimeKnives[Random.Range(0, player.inventory.runtimeKnives.Count)]);

            // 決めたナイフを対象の方向に投げる
            player.attack.ThrowKnife(randomKnife, Quaternion.FromToRotation(Vector2.up, dir));

            // 目的方向からちょっと右に動く
            await player.transform.DOMove(randomPoint + (Vector2)(Quaternion.Euler(0, 0, -70) * dir) * 3f, 0.1f).ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, cancellationToken: token);
        }

        if(targetPoint == (Vector2)basePoint) targetPoint = Vector2.zero;

        player.transform.position = (Vector2)basePoint + (targetPoint - (Vector2)basePoint).normalized * 16f;

        await player.transform.DOMove(basePoint, 0.5f).ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, cancellationToken: token);

        player.ctrler._rigidbody.bodyType = RigidbodyType2D.Dynamic;
        _fx_Trail.transform.parent = null;
        player.count_PermissionHit.Value--;
        player.enhancementRate_Power -= instantBuffAmount;
        player.count_Actable--;

        await UniTask.Delay(600, cancellationToken: token);
    }
}
