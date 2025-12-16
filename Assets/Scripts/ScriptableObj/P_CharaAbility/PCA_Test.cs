using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[CreateAssetMenu(fileName = "New_PCA", menuName = "Game Data/P_CharaAbility/Test")]
public class PCA_Test : Base_P_CharaAbility
{
    // キャラアビリティのテスト用データ、エフェクトを生成する

    [SerializeField] GameObject fx;

    public override UniTask ActivateAbility(CancellationToken token)
    {
        Instantiate(fx, player.transform.position, Quaternion.identity);

        return UniTask.CompletedTask;
    }
}
