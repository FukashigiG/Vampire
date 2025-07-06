using UnityEngine;

[CreateAssetMenu(fileName ="NewWeaponData", menuName = "Game Data/Weapon Data")]
public class KnifeData : ScriptableObject
{
    public enum ElementEnum
    {
        white, red, blue, yellow
    }

    public Sprite sprite;

    public string _name;

    public int rarity;

    public ElementEnum element;

    public int power;

    public GameObject prefab;

    public GameObject hitEffect;

    // ↓をつけるとエディタ上で長いテキストも扱いやすくなる
    [Multiline(6)] public string description;

    // 特殊効果
    public BaseHSpE[] specialEffects;

    // 以下キーワード能力
    /*
    public bool penetration; //貫通
    public bool defenceIgnore; // 防御無視
    public bool heavyBlow; // 重撃
    public bool thighSplitting; // 腿砕き
    public bool collapse; // 崩壊
    public bool blaze; // 火焔
    public bool freeze; // 氷結
    */
}
