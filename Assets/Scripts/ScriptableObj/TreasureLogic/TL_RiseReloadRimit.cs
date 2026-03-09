using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/TreasureLogic/RiseReloadRimit")]
public class TL_RiseReloadRimit : Base_TreasureLogic
{
    // 重複度N以上のナイフの基礎攻撃力を上げる

    [SerializeField] int requireCount;

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        int count = 0;
        bool onEffect = false;

        status.attack.onReload.Subscribe(x =>
        {
            if (onEffect)
            {
                status.enhancement_Limit_DrawKnife--;

                onEffect = false;
            }
            else
            {
                count++;

                if (count >= requireCount)
                {
                    onEffect = true;
                    count = 0;

                    status.enhancement_Limit_DrawKnife++;
                }

            }



        }).AddTo(disposables);
    }
}
