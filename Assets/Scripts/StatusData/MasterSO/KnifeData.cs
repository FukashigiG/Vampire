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
    [field: SerializeField] public List<KnifeAbility> abilities { get; private set; }

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

[System.Serializable]
public class KnifeAbility
{
    // やっぱ発動率と数値はアビリティごとの固有値にしたいわ

    // 発動する能力
    [SerializeField] public Base_KnifeAbilityLogic abilityLogic;
    // 発動率
    //[SerializeField] public int probability_Percent = 100; // デフォルト値
    // 効果倍率
    //[SerializeField] public float modifire = 1.0f; // デフォルト値
    // 効果ID 状態変化系効果の際等で使用される
    [SerializeField] public string effectID;

    // ナイフが投げられたときに呼ばれる
    public virtual void OnThrown(PlayerStatus status, GameObject knifeObj, KnifeData_RunTime knifeData)
    {
        // 変数がfalseなら無視
        if (! abilityLogic.effectOnThrown) return;

        // 1～100の乱数が発動確率以内なら、特殊効果を発動
        int randomNum = Random.Range(1, 101);

        if (randomNum <= abilityLogic.probability_Percent)
        {
            // 効果処理
            abilityLogic.ActivateAbility(status, knifeObj, knifeData, effectID);
        }
    }

    // ナイフがヒットした時に呼ばれる
    public virtual void OnHit(Base_MobStatus status, GameObject knifeObj, KnifeData_RunTime knifeData)
    {
        // 変数がfalseなら無視
        if (! abilityLogic.effectOnHit) return;

        // 1～100の乱数が発動確率以内なら、特殊効果を発動
        int randomNum = Random.Range(1, 101);

        if (randomNum <= abilityLogic.probability_Percent)
        {
            // 効果処理
            abilityLogic.ActivateAbility(status, knifeObj, knifeData, effectID);
        }
    }

    // コンストラクタ
    public KnifeAbility(Base_KnifeAbilityLogic abilityLogic, string effectID)
    {
        this.abilityLogic = abilityLogic;
        this.effectID = effectID;
    }
}
