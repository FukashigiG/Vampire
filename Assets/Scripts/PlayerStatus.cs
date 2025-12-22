using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : Base_MobStatus
{
    // 仮実装
    [SerializeField] StageDataHolder holder;

     public PlayerCharaData playerCharaData {  get; private set; }

    public PlayerInventory inventory { get; private set; }
    public PlayerAttack attack { get; private set; }

    [field : SerializeField] public int requiredEXP_LvUp {  get; private set; }

    // プレイヤー専用ステータス群
    int base_Luck;
    int base_EyeSight;
    float base_CoolTime_ThrowKnife = 0.15f;
    float base_Time_ReloadKnives = 1.5f; // ここ2つは基本この数値で固定なので数値割り当て済みにしてる
    int base_Limit_DrawKnife;
    

    public int enhancementRate_Luck = 0;
    public int enhancementRate_EyeSight = 0;
    public int enhancementRate_CoolTime_ThrowKnife = 0;
    public int enhancementRate_Time_ReloadKnives;
    public int enhancement_Limit_DrawKnife = 0;

    public int luck { get { return (int)(base_Luck * (1f + (enhancementRate_Defence / 100f))); } }
    public int eyeSight { get { return (int)(base_EyeSight * (1f + (enhancementRate_EyeSight / 100f))); } }
    public float coolTime_ThrowKnife { get { return (base_CoolTime_ThrowKnife * (1f + (enhancementRate_CoolTime_ThrowKnife / 100f))); } }
    public float time_ReloadKnives { get { return (base_Time_ReloadKnives * (1f + (enhancementRate_Time_ReloadKnives / 100f))); } }
    // ドロー上限のみ計算方法が異なり、増減量がパーセントでなくそのままの数値で扱われる
    public int limit_DrawKnife { get { return base_Limit_DrawKnife + enhancement_Limit_DrawKnife; } }


    public enum PlayerStatusType
    {
        atk, def, spd, eye, luk, lim
    }

    public List<Element> masteredElements { get; private set; } = new List<Element>();




    // 通知を飛ばす奴ら
    public ReactiveProperty<int> exp = new ReactiveProperty<int>();

    Subject<Unit> lvUp = new Subject<Unit>();
    public IObservable<Unit> onLvUp => lvUp;

    public IObservable<Unit> onSecond => subject_OnSecond;
    public IObservable<(Vector2 position, int amount)> onDamaged => subject_OnDamaged;
    public IObservable<(Base_MobStatus status, Base_StatusEffectData statusEffect, float duration, int amount)> onGetStatusEffect => subject_OnGetStatusEffect;
    public IObservable<(Base_MobStatus status, int value)> onDie=> subject_OnDie;

    // 購読のライフサイクルを管理するためのDisposable
    // これ１つで沢山のDisposableなやつらに対応可能らしい
    private CompositeDisposable disposables = new CompositeDisposable();

    protected override void Awake()
    {
        playerCharaData = holder.selectedChara;

        base.Awake();

        inventory = GetComponent<PlayerInventory>();
        attack = GetComponent<PlayerAttack>();

        //各内部ステータスをPlayerCharaDataから代入
        maxHP = playerCharaData.hp;
        _hitPoint.Value = maxHP;
        base_Power = playerCharaData.power; //　プレイヤーのpowerはナイフの速度を決定する
        base_Defence = playerCharaData.defense;
        base_MoveSpeed = playerCharaData.moveSpeed;

        base_Luck = playerCharaData.luck;
        base_EyeSight = playerCharaData.eyeSight;
        base_Limit_DrawKnife = playerCharaData.limit_DrawKnives;

        masteredElements.Add(playerCharaData.masteredElement);

        color_DamageTxt = Color.red;

        // キャラアビリティを渡す
        attack.SetCharaAbility(playerCharaData.charaAbility);
    }

    protected override void Start()
    {
        base.Start();

        // インベントリに初期所持ナイフと秘宝を追加
        foreach (var x in playerCharaData.initialKnives)
        {
            inventory.AddKnife(new KnifeData_RunTime(x));
        }
        foreach (var y in playerCharaData.initialTreasures)
        {
            inventory.AddTreasure(y);
        }

        //任意の敵が死んだらEXPゲット
        EnemyStatus.onDie.Subscribe(x => GetEXP(x.value)).AddTo( disposables );
    }

    // exp獲得
    public void GetEXP(int x)
    {
        exp.Value += x;

        // 経験値量が一定に達していたらレベルアップ
        if (exp.Value >= requiredEXP_LvUp) LvUp();
    }

    // レベルアップ
    void LvUp()
    {
        while (exp.Value >= requiredEXP_LvUp)
        {
            exp.Value -= requiredEXP_LvUp ;

            requiredEXP_LvUp += 1;
        }

        lvUp.OnNext(Unit.Default);

        // 獲得イベントを実行
        //GameEventDirector.Instance.TriggerEvent(GameEventDirector.Events.getTreasure);
    }

    // 攻撃を受ける処理
    public override bool GetAttack(int damagePoint, int elementPoint, Vector2 damagedPosi, bool isCritical = false, bool isIgnoreDefence = false)
    {
        bool x = base.GetAttack(damagePoint, elementPoint, damagedPosi, isCritical, isIgnoreDefence);

        if(x == true) BeInvincible(1f).Forget();

        return x;
    }

    // ダメージを受ける処理
    public override int TakeDamage(int value)
    {
        int trueValue = base.TakeDamage(value);

        return trueValue;
    }

    public override void HealHP(int x)
    {
        base.HealHP(x);
    }

    public void Modify_statusPoint(PlayerStatusType statusType, int point)
    {
        switch(statusType)
        {
            case PlayerStatusType.atk:
                this.base_Power += point;
                break;

            case PlayerStatusType.def:
                this.base_Defence += point;
                break;

            case PlayerStatusType.spd:
                this.base_MoveSpeed += point;
                break;

            case PlayerStatusType.eye:
                this.base_EyeSight += point;
                break;

            case PlayerStatusType.luk:
                this.base_Luck += point;
                break;

            case PlayerStatusType.lim:
                this.base_Limit_DrawKnife += point;
                break;
        }
    }

    // 指定秒数間むてきになる
    async UniTask BeInvincible(float sec)
    {
        count_PermissionHit += 1;

        try
        {
            await UniTask.Delay((int)(sec * 1000));
        }
        catch
        {

        }
        finally
        {
            count_PermissionHit -= 1;
        }
    }

    public override UniTask Die()
    {
        subject_OnDie.OnNext((this, 1));

        this.gameObject.SetActive(false);

        return UniTask.CompletedTask;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        disposables.Dispose();
    }
}
