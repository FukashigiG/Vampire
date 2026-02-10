using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBossEnemyAct", menuName = "Game Data/BossEnemyAct/Basic/Warp")]
public class BEA_Warp : Base_BossEnemyAct
{
    [SerializeField] float radius_RandomPoint;

    public async override UniTask Action(EnemyCtrler_BigBoss ctrler, CancellationToken token)
    {
        ctrler._enemyStatus.count_PermissionHit.Value++;

        try
        {
            // 下はanimatorを使ってるせいで無効
            //await ctrler.gameObject.transform.GetChild(0).DOScale(Vector3.zero, 0.4f).ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, token);

            ctrler._animator.SetTrigger("Hide");

            await UniTask.Delay(1000, cancellationToken: token);

            // ターゲットを中心とする半径Nの円周上の点を取得
            // 半径１の円の内側のランダムな点を正規化して円周上の点として取得したものに半径分の数値をかけ、ターゲットの座標と合成
            Vector2 pos = Random.insideUnitCircle.normalized * radius_RandomPoint + (Vector2)ctrler.target.position;

            ctrler._animator.SetTrigger("Appear");

            ctrler.transform.position = pos;
        }
        catch
        {
            ctrler._animator.SetTrigger("Appear");

            ctrler.gameObject.transform.GetChild(0).transform.localScale = Vector2.one;

            ctrler._enemyStatus.count_PermissionHit.Value--;

            throw;
        }

        ctrler._enemyStatus.count_PermissionHit.Value--;

        //await ctrler.gameObject.transform.GetChild(0).DOScale(Vector3.one, 0.4f).ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, token);

        await UniTask.Delay(1000, cancellationToken: token);
    }
}
