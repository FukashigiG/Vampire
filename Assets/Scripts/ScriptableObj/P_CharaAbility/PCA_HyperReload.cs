using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New_PCA", menuName = "Game Data/P_CharaAbility/HyperReload")]
public class PCA_HyperReload : Base_P_CharaAbility
{
    // 発動すると即座にリロードし、引いたナイフを強化？

    public override void ActivateAbility(PlayerStatus status)
    {
        status.attack.StartAttakLoop();
    }
}
