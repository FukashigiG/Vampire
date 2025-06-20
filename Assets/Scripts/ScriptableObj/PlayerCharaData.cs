using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerCharaData", menuName = "Game Data/Player Chara Data")]
public class PlayerCharaData : ScriptableObject
{
    public Sprite sprite;

    public string _name;

    public int hp;

    public float moveSpeed;
    public float luck;
    public float throwPower;
    public float defense;

    public GameObject prefab;
}
