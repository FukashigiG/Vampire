using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Game Data/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public Sprite sprite;

    public string _name;

    public int hp;
    public int power;
    public int defense;

    public float moveSpeed;
    public float weight;

    public float amount_EXP;

    public GameObject prefab;

    [Serializable] public class DropItem
    {
        public GameObject prefab;

        public int dropRate_Parcentage;
    }

    public DropItem[] dropItems;
}
