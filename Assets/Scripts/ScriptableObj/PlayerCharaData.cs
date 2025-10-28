using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerCharaData", menuName = "Game Data/Player Chara Data")]
public class PlayerCharaData : ScriptableObject
{
    public Sprite sprite;

    public string _name;

    public int hp;
    public int power;
    public int defense;
    public int moveSpeed;
    public float luck;
    public float eyeSight;

    public KnifeData[] initialKnives;
    public Base_TreasureData[] initialTreasures;

    public KnifeData.ElementEnum masteredElement;

    public GameObject prefab;
}
