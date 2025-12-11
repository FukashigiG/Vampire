using System;
using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Game Data/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [field: SerializeField] public Sprite sprite { get; private set; }

    [field: SerializeField] public string _name {  get; private set; }
    [field: SerializeField, Multiline(6)] public string description { get; private set; }

    [field: SerializeField, Range(1, 3)] public int rank { get; private set; } = 1;


    public enum EnemyActType
    {
        Infight, Shooter, BigBoss
    }

    [field: SerializeField] public EnemyActType actType {  get; private set; }

    [field: SerializeField] public int hp {  get; private set; }
    [field: SerializeField] public int power {  get; private set; }
    [field: SerializeField] public int defense {  get; private set; }

    [field: SerializeField] public float moveSpeed {  get; private set; }

    // もしシュータータイプなら、射程等の項目も表示する
    // showIfを使ってると{get;set;}が使えないので、別途参照用のプロパティを用意
    public enum ShotType
    {
       OneShot, RapidFire
    }

    [ShowIf("actType", EnemyActType.Shooter), SerializeField] ShotType _shotType = ShotType.OneShot;
    public ShotType shotType => _shotType;

    // 発射する弾の数
    [ShowIf("actType", EnemyActType.Shooter), SerializeField, Range(1, 20)] int _num_Bullet = 1;
    public int num_Bullet => _num_Bullet;

    // 弾の発散角度
    [ShowIf("actType", EnemyActType.Shooter), SerializeField, Range(0, 360)] float _divergenceAngle = 0;
    public float divergenceAngle => _divergenceAngle;

    // 攻撃対象探知距離
    [ShowIf("actType", EnemyActType.Shooter), SerializeField] float _range_Shot = 0;
    public float range_Shot => _range_Shot;

    // 発射頻度
    [ShowIf("actType", EnemyActType.Shooter), SerializeField] float _friquentry_Shot = 0;
    public float friquentry_Shot => _friquentry_Shot;

    // 弾となるオブジェクトのプレハブ
    [ShowIf("actType", EnemyActType.Shooter), SerializeField] GameObject _bulletPrefab;
    public GameObject bulletPrefab => _bulletPrefab;

    [field: SerializeField] public float amount_EXP {  get; private set; }

    [field: SerializeField] public List<Base_EnemyStatusAbilityData> statusAbilities {  get; private set; }

    [Serializable] public class DropItem
    {
        public GameObject prefab;

        public int dropRate_Parcentage;
    }

    public DropItem[] dropItems;

    [Serializable] public class BossActionData
    {
        [field: SerializeField] public Base_BossEnemyAct actionLogic { get; private set; }

        // 技の発動条件
        public enum TriggerType
        {
            WeightRandom, // 重み付きランダム このタイプの技は何回でも使用される
            SpecifiedActCount, // 指定回数目の行動時（初回、N回目の行動）発動は1回のみ
            HpThreshold // HPが一定以下になったら 同じく発動は1回のみ
        }
        [field: SerializeField] public TriggerType triggerType {  get; private set; }

        // 行動数トリガーのときのみ表示...したかったなあ
        [field: SerializeField] public int targetActCount {  get; private set; }

        // HPトリガーの時のみ表示...したかったなあ
        [field: SerializeField, Range(0, 100)] public int thresholdHpRate_Percent { get; private set; } = 50;

        //[field: SerializeField] public bool isOneTimeOnry {  get; private set; }

        [field: SerializeField, Range(0, 100)] public int baseWeight { get; private set; } = 50;
        [field: SerializeField, Range(0.1f, 0.9f)] public float decayRate { get; private set; } = 0.5f;
    }

    [ShowIf("isBigBoss"), SerializeField] List<BossActionData> _bossActions;
    public List<BossActionData> bossActions => _bossActions;
}