using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Game Data/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public Sprite sprite;

    public string _name;

    public int hp;

    public float moveSpeed;
    public float defense;

    public GameObject prefab;
}
