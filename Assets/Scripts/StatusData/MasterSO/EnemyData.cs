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

    public enum EnemyRank
    {
        Normal, Boss, BigBoss, Event
    }

    [field: SerializeField] public EnemyRank rank { get; private set; }


    public enum EnemyActType
    {
        Infight, Shooter
    }

    [field: SerializeField] public EnemyActType actType {  get; private set; }
    bool isInfight => actType == EnemyActType.Infight;
    bool isShooter => actType == EnemyActType.Shooter;

    [field: SerializeField] public int hp {  get; private set; }
    [field: SerializeField] public int power {  get; private set; }
    [field: SerializeField] public int defense {  get; private set; }

    [field: SerializeField] public float moveSpeed {  get; private set; }

    // もしシュータータイプなら、射程の項目も表示する
    // showIfを使ってると{get;set;}が使えないので、別途参照用のプロパティを用意
    [ShowIf("isShooter"), SerializeField] float _range_Shot = 0;
    public float range_Shot => _range_Shot;

    [ShowIf("isShooter"), SerializeField] float _friquentry_Shot = 0;
    public float friquentry_Shot => _friquentry_Shot;

    [ShowIf("isShooter"), SerializeField] GameObject _bulletPrefab;
    public GameObject bulletPrefab => _bulletPrefab;

    [field: SerializeField] public float amount_EXP {  get; private set; }

    [field: SerializeField] public List<Base_EnemyStatusAbilityData> statusAbilities {  get; private set; }

    [field: SerializeField] public GameObject prefab {  get; private set; }

    [Serializable] public class DropItem
    {
        public GameObject prefab;

        public int dropRate_Parcentage;
    }

    public DropItem[] dropItems;
}
