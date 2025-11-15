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

    public IObservable<(Vector2 position, int amount)> onDamaged => subject_OnDamaged;
    public static IObservable<(Base_MobStatus status, Base_StatusEffectData effect, float duration, int amount)> onGetStatusEffect => subject_OnGetStatusEffect;
    public static IObservable<(Base_MobStatus status, int value)> onDie => subject_OnDie;

    // オブジェクトが破棄されたときに呼ばれる処理
    // 死亡処理とはまた違うので、MiniMapにてオブジェクトが死亡以外で消えても反応できるように
    static Subject<EnemyStatus> subject_OnDestroy = new();
    public static IObservable<EnemyStatus> onDestroy => subject_OnDestroy;

    protected override void Start()
    {
        base.Start();

        GetComponent<SpriteRenderer>().sprite = _enemyData.sprite;
    }

    public void Initialize(float multiplier)
    {
        maxHP = (int)((float)_enemyData.hp * multiplier);
        _hitPoint.Value = maxHP;

        base_Power = (int)(_enemyData.power * multiplier);
        base_Defence = (int)(_enemyData.defense * multiplier);
        base_MoveSpeed = (int)(_enemyData.moveSpeed * multiplier);
    }

    public override void TakeDamage(int value)
    {
        base.TakeDamage(value);

        //ダメージテキストを出す処理
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);

        var x = Instantiate(damageTxt, screenPoint, Quaternion.identity, GameObject.Find("Parent_DamageTxt").transform);

        x.GetComponent<DamageTxtCtrler>().Initialize(value);
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

        base.OnDestroy();
    }
}
