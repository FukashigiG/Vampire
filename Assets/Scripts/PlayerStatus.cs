using Cysharp.Threading.Tasks;
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

    public List<Element> masteredElements { get; private set; } = new List<Element>();

    int exp;

    bool isInvincible;

    public Subject<Unit> lvUp = new Subject<Unit>();

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
        hitPoint = maxHP;
        base_Power = playerCharaData.power; //　プレイヤーのpowerはナイフの速度を決定する
        base_Defence = playerCharaData.defense;
        base_MoveSpeed = playerCharaData.moveSpeed;

        luck = playerCharaData.luck;
        eyeSight = playerCharaData.eyeSight;

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
        base.GetAttack(damagePoint, elementPoint, damagedPosi, isCritical, isIgnoreDefence);

        BeInvincible(1f).Forget();
    }

    // ダメージを受ける処理
    public override void TakeDamage(int value)
    {
        base.TakeDamage(value);

        gauge_HP.fillAmount = (float)hitPoint / (float)maxHP;
    }

    public override void HealHP(int x)
    {
        base.HealHP(x);

        Debug.Log(x);

        gauge_HP.fillAmount = (float)hitPoint / (float)maxHP;
    }

    // 指定秒数間むてきになる
    async UniTask BeInvincible(float sec)
    {
        try
        {
            isInvincible = true;

            await UniTask.Delay((int)(sec * 1000));

            isInvincible = false;
        }
        catch
        {

        }
        finally
        {

        }
    }

    public override void Die()
    {
        onDie.OnNext((this, 1));

        this.gameObject.SetActive(false);

        panel_Die.SetActive(true);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        disposables.Dispose();
    }
}
