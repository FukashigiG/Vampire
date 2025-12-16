using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Threading;

[CreateAssetMenu(fileName = "New_PCA", menuName = "Game Data/P_CharaAbility/HyperReload")]
public class PCA_HyperReload : Base_P_CharaAbility
{
    // 発動すると即座にリロードし、引いたナイフを強化、おまけにHPを割合回復

    [SerializeField] int ratio_EnhanceKnifePower_Parcent;
    [SerializeField] int ratio_Heal_Parcent;

    bool onEnhance = false;

    public override void Initialize(PlayerStatus status)
    {
        base.Initialize(status);

        player.attack.onReload.Subscribe(_list =>
        {
            if (! onEnhance) return;

            // 各ナイフの基礎攻撃力をN倍
            foreach (var knifeData in _list)
            {
                int enhanceAmount = (int)((float)knifeData.power * ratio_EnhanceKnifePower_Parcent / 100f);

                knifeData.power += enhanceAmount;
            }

            onEnhance = false;

        }).AddTo(player);
    }

    public override UniTask ActivateAbility(CancellationToken token)
    {
        onEnhance = true;

        player.attack.StartAttakLoop();

        player.HealHP((int)((float)player.maxHP * (float)ratio_Heal_Parcent / 100f));

        return UniTask.CompletedTask;
    }
}
