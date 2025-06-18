using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : Base_MobStatus
{
    [SerializeField] Image gauge_EXP;

    [SerializeField] int requiredEXP_LvUp;

    int exp;


    public Subject<Unit> lvUp = new Subject<Unit>();

    // 購読のライフサイクルを管理するためのDisposable
    private CompositeDisposable disposables = new CompositeDisposable();

    protected override void Start()
    {
        base.Start();

        EnemyStatus.onDie.Subscribe(x => GetEXP(x)).AddTo( disposables );
    }

    public void GetEXP(int x)
    {
        exp += x;

        if (exp >= requiredEXP_LvUp) LvUp();

        gauge_EXP.fillAmount = (float)exp / (float)requiredEXP_LvUp;
    }

    void LvUp()
    {
        while (exp >= requiredEXP_LvUp)
        {
            exp -= requiredEXP_LvUp ;

            requiredEXP_LvUp += 1;
        }

        lvUp.OnNext(Unit.Default);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        disposables.Dispose();
    }
}
