using UnityEngine;

[CreateAssetMenu(fileName ="NewWeaponData", menuName = "Game Data/Weapon Data")]
public class KnifeData : ScriptableObject
{
    public Sprite sprite;

    public int Rarity;

    public int power;

    public GameObject prefab;
}
