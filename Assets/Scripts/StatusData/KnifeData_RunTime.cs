using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections.ObjectModel;

// カスタムリスト
// 基本的な挙動はList<BaseHSpE>と同じだが、Add（～～）された際に同種の重複が起きるなら無視する
public class CustomList : Collection<KnifeAbility>
{
    // Add（）によってなにかしらが加えられる際の処理
    protected override void InsertItem(int index, KnifeAbility item)
    {
        // その加えられるものの型と同じものが既にあるかどうか判別
        bool hasTargetTypeAbility = this.Any(ability => ability.abilityLogic.GetType() == item.abilityLogic.GetType());

        if (hasTargetTypeAbility) return;

        base.InsertItem(index, item);
    }
}

public class KnifeData_RunTime
{
    public Sprite sprite { get; private set; }
    public Sprite sprite_Defolme { get; private set; }
    public Color color { get; private set; }

    public string _name { get; private set; }

    public int rarity { get; private set; }

    public Element element;

    public int power;
    public int elementPower;

    public GameObject prefab { get; private set; }

    public GameObject hitEffect;

    // ↓をつけるとエディタ上で長いテキストも扱いやすくなる
    [Multiline(6)] public string description;

    // 特殊効果
    public CustomList abilities {  get; private set; } = new CustomList();

    // ランタイム用でないナイフデータを元にするコンストラクタ
    public KnifeData_RunTime(KnifeData data)
    {
        sprite = data.sprite;
        sprite_Defolme = data.sprite_Defolme;
        color = data.color;
        _name = data._name;
        rarity = data.rarity;
        element = data.element;
        power = data.power;
        elementPower = data.elementPower;
        prefab = data.prefab;
        hitEffect = data.hitEffect;
        description = data.description;

        // ナイフに登録されていた各HSpEを初期化したものをリストに加える
        foreach (var ability in data.abilities)
        {
            if(ability.abilityLogic != null)
            {
                // 参照(ability)をそのままAddするのではなく、
                // 新しいKnifeAbilityインスタンスを生成してAddする
                KnifeAbility newAbility = new KnifeAbility(
                    UnityEngine.Object.Instantiate(ability.abilityLogic),
                    ability.probability_Percent,
                    ability.modifire,
                    ability.effectID
                );
                abilities.Add(newAbility);
            }
        }
    }

    // このクラスを元にするコンストラクタ
    public KnifeData_RunTime(KnifeData_RunTime data)
    {
        sprite = data.sprite;
        sprite_Defolme = data.sprite_Defolme;
        color = data.color;
        _name = data._name;
        rarity = data.rarity;
        element = data.element;
        power = data.power;
        elementPower = data.elementPower;
        prefab = data.prefab;
        hitEffect = data.hitEffect;
        description = data.description;

        foreach (var ability in data.abilities)
        {
            if (ability.abilityLogic != null)
            {
                // 参照(ability)をそのままAddするのではなく、
                // 新しいKnifeAbilityインスタンスを生成してAddする
                KnifeAbility newAbility = new KnifeAbility(
                    UnityEngine.Object.Instantiate(ability.abilityLogic),
                    ability.probability_Percent,
                    ability.modifire,
                    ability.effectID
                );
                abilities.Add(newAbility);
            }
        }
    }
}
