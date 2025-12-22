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

    public void Initialize(EnemyData data, float multiplier)
    {
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

    public async override UniTask Die()
    {
        count_PermissionDamage++;
        count_PermissionHit++;
        count_Actable++;

        subject_OnDie.OnNext((this, 1));

        if(_enemyData.actType == EnemyActType.BigBoss) await transform.DOShakePosition(2 * Time.timeScale, strength:1f, vibrato: 24).ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, gameObject.GetCancellationTokenOnDestroy());

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

        Destroy(this.gameObject);
    }

    protected override void OnDestroy()
    {
        subject_OnDestroy.OnNext(this);

        if(disposables != null)　disposables.Dispose();

        base.OnDestroy();
    }
}
