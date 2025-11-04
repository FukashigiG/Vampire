using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System.Linq;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/Treasure Data/DefeatCount_Heal")]
public class DefeatCount_HealTreasure : Base_TreasureData
{
    // ŠŽ‚µ‚Ä‚¢‚éŠÔA“G‚ðN‘Ì“|‚·‚½‚Ñ‰ñ•œ

    [SerializeField] int countCycle;
    [SerializeField] int healRatio_Percent;

    public override void OnAdd(PlayerStatus status)
    {

    }

    public override void OnRemove(PlayerStatus status)
    {

    }

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        int count = 0;

        EnemyStatus.onDie.Subscribe(x =>
        {
            count++;

            if(count >= countCycle)
            {
                status.HealHP((int)(status.maxHP * (float)(healRatio_Percent / 100)));

                count = 0;
            }

        }).AddTo(disposables);
    }
}
