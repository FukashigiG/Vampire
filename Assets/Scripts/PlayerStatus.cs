using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : Base_MobStatus
{
    [SerializeField] Image gauge_EXP;

    [SerializeField] PlayerCharaData playerCharaData;

    [SerializeField] int requiredEXP_LvUp;

    public float moveSpeed {  get; private set; }
    public float defense {  get; private set; }
    public float throwPower { get; private set; }
    public float luck {  get; private set; }

    int exp;


    public Subject<Unit> lvUp = new Subject<Unit>();

    // 購読のライフサイクルを管理するためのDisposable
    // これ１つで沢山のDisposableなやつらに対応可能らしい
    private CompositeDisposable disposables = new CompositeDisposable();

    protected override void Start()
    {
        base.Start();

        //各内部ステータスをPlayerCharaDataから代入
        hitPoint = playerCharaData.hp;
        moveSpeed = playerCharaData.moveSpeed;
        defense = playerCharaData.defense;
        throwPower = playerCharaData.throwPower;
        luck = playerCharaData.luck;

        //任意の敵が死んだらEXPゲット
        EnemyStatus.onDie.Subscribe(x => GetEXP(x)).AddTo( disposables );
    }

    // exp獲得
    public void GetEXP(int x)
    {
        exp += x;

        // 経験値量が一定に達していたらレベルアップ
        if (exp >= requiredEXP_LvUp) LvUp();

        // 経験値ゲージUI更新
        gauge_EXP.fillAmount = (float)exp / (float)requiredEXP_LvUp;
    }

    // レベルアップ
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
