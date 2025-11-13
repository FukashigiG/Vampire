using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New_PCA", menuName = "Game Data/P_CharaAbility/Test")]
public class PCA_Test : Base_P_CharaAbility
{
    // キャラアビリティのテスト用データ、エフェクトを生成する

    [SerializeField] GameObject fx;

    public override void ActivateAbility(PlayerStatus status)
    {
        Instantiate(fx, status.transform.position, Quaternion.identity);
    }
}
