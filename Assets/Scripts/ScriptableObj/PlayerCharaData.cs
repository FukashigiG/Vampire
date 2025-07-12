using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerCharaData", menuName = "Game Data/Player Chara Data")]
public class PlayerCharaData : ScriptableObject
{
    public Sprite sprite;

    public string _name;

    public int hp;
    public int defense;

    public float moveSpeed;
    public float luck;
    public float throwPower;
    public float eyeSight;

    public KnifeData[] initialKnives;
    public Base_TreasureData[] initialTreasures;

    public KnifeData.ElementEnum masteredElement;

    public GameObject prefab;
}
