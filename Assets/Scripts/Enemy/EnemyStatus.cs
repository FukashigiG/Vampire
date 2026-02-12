using Cysharp.Threading.Tasks;
using DG.Tweening;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using static EnemyData;
using Random = UnityEngine.Random;

public class EnemyStatus : Base_MobStatus
{
    [SerializeField] GameObject fx_Die;

    public EnemyData _enemyData { get; private set; }
    public Base_EnemyCtrler ctrler {  get; private set; }

    public EnemyData.ShotType shotType {  get; private set; }
    public int num_Bullet {  get; private set; }
    public float divergenceAngle {  get; private set; }
    public float range_Shot { get; private set; } = 0f;
    public float friquentry_Shot { get; private set; } = 0f;
    public GameObject bullet_Prefab { get; private set; }

    // ダメージを受けたとき、状態変化を受けたとき、死んだときに出す通知の購読部分
    // 死亡通知はstaticなものとそうでないものの両方あると嬉しいので両方ある
    public IObservable<(Vector2 position, int amount)> onDamaged => subject_OnDamaged;
    public static IObservable<(Base_MobStatus status, Base_StatusEffectData effect, float duration, int amount)> onGetStatusEffect => subject_OnGetStatusEffect;
    public IObservable<(Base_MobStatus status, int value)> onDie => subject_OnDie;

    public static Subject<EnemyStatus> subject_OnDie_Static = new Subject<EnemyStatus>();
    public static IObservable<EnemyStatus> onDie_Static => subject_OnDie_Static;

    // オブジェクトが破棄されたときに呼ばれる処理
    // 死亡処理とはまた違うので、MiniMapにてオブジェクトが死亡以外で消えても反応できるように
    static Subject<EnemyStatus> subject_OnDestroy = new();
    public static IObservable<EnemyStatus> onDestroy => subject_OnDestroy;


    // 主にエネミーアビリティ用の
    CompositeDisposable disposables = new CompositeDisposable();

    public void Initialize_OR(EnemyData data, float multiplier)
    {
        base.Initialize();

        ctrler = GetComponent<Base_EnemyCtrler>();

        // ステータスデータの取得
        _enemyData = data;

        // ミニマップに自身を登録させる
        MiniMapController.Instance.NewEnemyInstance(this);

        color_DamageTxt = Color.white;

        maxHP = (int)((float)_enemyData.hp * multiplier);
        _hitPoint.Value = maxHP;

        base_Power = (int)(_enemyData.power * multiplier);
        base_Defence = (int)(_enemyData.defense * multiplier);
        base_MoveSpeed = (int)(_enemyData.moveSpeed * multiplier);

        // もしシューター型なら射程、発射頻度等も代入
        if(_enemyData.actType == EnemyData.EnemyActType.Shooter)
        {
            shotType = _enemyData.shotType;
            num_Bullet = _enemyData.num_Bullet;
            divergenceAngle = _enemyData.divergenceAngle;
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

        ctrler.Initialize();
    }

    public override void Die()
    {
        count_PermissionDamage++;
        count_PermissionHit.Value++;
        count_Actable++;

        subject_OnDie_Static.OnNext(this);

        base.Die();
    }

    protected override void OnDestroy()
    {
        subject_OnDestroy.OnNext(this);

        if(disposables != null)　disposables.Dispose();

        base.OnDestroy();
    }
}
