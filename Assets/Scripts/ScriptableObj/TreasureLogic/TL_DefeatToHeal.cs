using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System.Linq;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/TreasureLogic/DefeatCount_Heal")]
public class TL_DefeatToHeal : Base_TreasureLogic
{
    // ŠŽ‚µ‚Ä‚¢‚éŠÔA“G‚ðN‘Ì“|‚·‚½‚Ñ‰ñ•œ

    [SerializeField] int countCycle;
    [SerializeField] int healRatio_Percent;

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        int count = 0;

        EnemyStatus.onDie_Static.Subscribe(x =>
        {
            count++;

            if(count >= countCycle)
            {
                status.HealHP((int)(status.maxHP * (float)(healRatio_Percent / 100)));

                subject_OnAct.OnNext(Unit.Default);

                count = 0;
            }

        }).AddTo(disposables);
    }
}
