using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyStatus : Base_MobStatus
{
    [SerializeField] EnemyData _enemyData;
    [SerializeField] GameObject fx_Die;
    [SerializeField] GameObject damageTxt;

    public Base_EnemyCtrler ctrler {  get; private set; }

    public float range_Shot { get; private set; } = 0f;
    public float friquentry_Shot { get; private set; } = 0f;
    public GameObject bullet_Prefab { get; private set; }

    public IObservable<(Vector2 position, int amount)> onDamaged => subject_OnDamaged;
    public static IObservable<(Base_MobStatus status, Base_StatusEffectData effect, float duration, int amount)> onGetStatusEffect => subject_OnGetStatusEffect;
    public static IObservable<(Base_MobStatus status, int value)> onDie => subject_OnDie;

    // オブジェクトが破棄されたときに呼ばれる処理
    // 死亡処理とはまた違うので、MiniMapにてオブジェクトが死亡以外で消えても反応できるように
    static Subject<EnemyStatus> subject_OnDestroy = new();
    public static IObservable<EnemyStatus> onDestroy => subject_OnDestroy;


    // 主にエネミーアビリティ用の
    CompositeDisposable disposables = new CompositeDisposable();

    protected override void Awake()
    {
        base.Awake();

        ctrler = GetComponent<Base_EnemyCtrler>();
    }

    protected override void Start()
    {
        base.Start();

        GetComponent<SpriteRenderer>().sprite = _enemyData.sprite;

        // 自分を初期化
        Initialize(1 + (GameAdmin.Instance.waveCount - 1) * GameAdmin.Instance.waveBoostMultiplier);

        // ミニマップに自身を登録させる
        MiniMapController.Instance.NewEnemyInstance(this);
    }

    public void Initialize(float multiplier)
    {
        maxHP = (int)((float)_enemyData.hp * multiplier);
        _hitPoint.Value = maxHP;

        base_Power = (int)(_enemyData.power * multiplier);
        base_Defence = (int)(_enemyData.defense * multiplier);
        base_MoveSpeed = (int)(_enemyData.moveSpeed * multiplier);

        // もしシューター型なら射程、発射頻度も代入
        if(_enemyData.actType == EnemyData.EnemyActType.Shooter)
        {
            range_Shot = _enemyData.range_Shot;
            friquentry_Shot = _enemyData.friquentry_Shot;
            bullet_Prefab = _enemyData.bulletPrefab;
        }

        // 各アビリティに対して
        foreach(var ability in _enemyData.statusAbilities)
        {
            if (ability == null) return;

            // 起動
            ability.ApplyAbility(this, disposables);
        }
    }

    public override int TakeDamage(int value)
    {
        // base内で引数の値が変動するため、こういう書き方をしている
        int trueValue = base.TakeDamage(value);

        //ダメージテキストを出す処理
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);

        var x = Instantiate(damageTxt, screenPoint, Quaternion.identity, GameObject.Find("Parent_DamageTxt").transform);

        x.GetComponent<DamageTxtCtrler>().Initialize(trueValue);

        return trueValue;
    }

    public override void Die()
    {
        // 各ドロップアイテム抽選
        if(_enemyData.dropItems.Length > 0)
        {
            foreach(var item in _enemyData.dropItems)
            {
                int randomCount = Random.Range(1, 101);

                if (randomCount < item.dropRate_Parcentage) Instantiate(item.prefab, transform.position, Quaternion.identity);
            }
        }

        // 死亡エフェクト
        Instantiate(fx_Die, transform.position, Quaternion.identity);

        base.Die();
    }

    protected override void OnDestroy()
    {
        subject_OnDestroy.OnNext(this);

        if(disposables != null)　disposables.Dispose();

        base.OnDestroy();
    }
}
