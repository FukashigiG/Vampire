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

    [SerializeField] GameObject spirit_Prefab;

    async public override UniTask ActivateAbility(CancellationToken token)
    {
        for (int i = 0;i < num_SummonSpilit; i++)
        {
            Instantiate(spirit_Prefab, PlayerController.Instance.transform.position, Quaternion.identity);
        }



        await UniTask.Delay((int)(600), cancellationToken: token);
    }
}
