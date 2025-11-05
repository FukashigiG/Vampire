using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewWeaponData", menuName = "Game Data/Weapon Data")]
public class KnifeData : ScriptableObject
{
    // ナイフの各ステータスをまとめたもの
    // このスクリプトはUnityエディタ上に登録するためのものであり、ゲーム中では_RunTimeがついてるものを扱う
    // これらの変数は外部からの参照と、インスペクター上での書き換えが出来る

    [field: SerializeField] public Sprite sprite { get; private set; }
    [field: SerializeField] public Sprite sprite_Defolme { get; private set; }
    [field: SerializeField] public Color color { get; private set; }

    [field: SerializeField] public string _name { get; private set; }

    [field: SerializeField] public int rarity { get; private set; }

    [field: SerializeField] public Element element { get; private set; }

    [field: SerializeField] public int power { get; private set; }
    [field: SerializeField] public int elementPower { get; private set; }

    [field: SerializeField] public GameObject prefab { get; private set; }

    [field: SerializeField] public GameObject hitEffect { get; private set; }

    // ↓をつけるとエディタ上で長いテキストも扱いやすくなる
    [Multiline(6)] public string description;

    // 特殊効果
    [field: SerializeField] public List<Base_KnifeAbility> abilities { get; private set; }

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
