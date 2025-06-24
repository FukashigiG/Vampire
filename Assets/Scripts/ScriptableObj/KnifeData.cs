using UnityEngine;

[CreateAssetMenu(fileName ="NewWeaponData", menuName = "Game Data/Weapon Data")]
public class KnifeData : ScriptableObject
{
    public Sprite sprite;

    public string _name;

    public int rarity;

    public int power;

    public GameObject prefab;

    // ↓をつけるとエディタ上で長いテキストも扱いやすくなる
    [Multiline(6)] public string description;
}
