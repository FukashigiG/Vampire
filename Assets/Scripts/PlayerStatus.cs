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
    [SerializeField] GameObject panel_Die;

    [SerializeField] Image gauge_EXP;
    [SerializeField] Image gauge_HP;

    public PlayerCharaData playerCharaData;

    public PlayerInventory inventory { get; private set; }
    public PlayerAttack attack { get; private set; }

    [SerializeField] int requiredEXP_LvUp;

    public float luck;
    public float eyeSight;
    public int limit_DrawKnife;

    bool isInvincible;

    public enum PlayerStatusType
    {
        atk, def, spd, eye, luk, lim
    }

    public List<Element> masteredElements { get; private set; } = new List<Element>();

    int exp;

    Subject<Unit> lvUp = new Subject<Unit>();

    public IObservable<(Vector2 position, int amount)> onDamaged => subject_OnDamaged;
    public IObservable<(Base_MobStatus status, StatusEffectType type, float duration, int amount)> onGetStatusEffect => subject_OnGetStatusEffect;
    public IObservable<(Base_MobStatus status, int value)> onDie=> subject_OnDie;

    // 購読のライフサイクルを管理するためのDisposable
    // これ１つで沢山のDisposableなやつらに対応可能らしい
    private CompositeDisposable disposables = new CompositeDisposable();

    protected override void Awake()
    {
        base.Awake();

        inventory = GetComponent<PlayerInventory>();
        attack = GetComponent<PlayerAttack>();

        //各内部ステータスをPlayerCharaDataから代入
        maxHP = playerCharaData.hp;
        _hitPoint.Value = maxHP;
        base_Power = playerCharaData.power; //　プレイヤーのpowerはナイフの速度を決定する
        base_Defence = playerCharaData.defense;
        base_MoveSpeed = playerCharaData.moveSpeed;

        luck = playerCharaData.luck;
        eyeSight = playerCharaData.eyeSight;
        limit_DrawKnife = playerCharaData.limit_DrawKnives;

        masteredElements.Add(playerCharaData.masteredElement);
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

        // 獲得イベントを実行
        GameEventDirector.Instance.TriggerEvent(GameEventDirector.Events.getTreasure);
    }

    // 攻撃を受ける処理
    public override void GetAttack(int damagePoint, int elementPoint, Vector2 damagedPosi, bool isCritical = false, bool isIgnoreDefence = false)
    {
        if(isInvincible) return;

        base.GetAttack(damagePoint, elementPoint, damagedPosi, isCritical, isIgnoreDefence);

        BeInvincible(1f).Forget();
    }

    // ダメージを受ける処理
    public override void TakeDamage(int value)
    {
        base.TakeDamage(value);
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
                this.eyeSight += point;
                break;

            case PlayerStatusType.luk:
                this.luck += point;
                break;

            case PlayerStatusType.lim:
                this.limit_DrawKnife += point;
                break;
        }
    }

    // 指定秒数間むてきになる
    async UniTask BeInvincible(float sec)
    {
        isInvincible = true;

        try
        {
            await UniTask.Delay((int)(sec * 1000));
        }
        catch
        {

        }
        finally
        {
            isInvincible = false;
        }
    }

    public override void Die()
    {
        subject_OnDie.OnNext((this, 1));

        this.gameObject.SetActive(false);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        disposables.Dispose();
    }
}
