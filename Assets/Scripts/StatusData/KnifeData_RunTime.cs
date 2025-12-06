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
        // その加えられるものと同じものが既にあるかどうか判別
        bool hasTargetTypeAbility = this.Any(ability => ability.abilityLogic.effectName == item.abilityLogic.effectName);

        if (hasTargetTypeAbility) return;

        base.InsertItem(index, item);
    }
}

public class KnifeData_RunTime : Base_PlayerItem
{
    public int power;
    public int elementPower;

    // 所持時のナイフ重複度数
    [Range(0, 10)] public int count_Multiple = 1;

    public GameObject prefab { get; private set; }

    public GameObject hitEffect;

    // 特殊効果
    public CustomList abilities {  get; private set; } = new CustomList();

    // ランタイム用でないナイフデータを元にするコンストラクタ
    public KnifeData_RunTime(KnifeData data)
    {
        sprite = data.sprite;
        _name = data._name;
        rank = data.rank;
        element = data.element;
        power = data.power;
        elementPower = data.elementPower;
        prefab = data.prefab;
        hitEffect = data.hitEffect;
        flavortext = data.flavortext;

        // ナイフに登録されていた各HSpEを初期化したものをリストに加える
        foreach (var ability in data.abilities)
        {
            if(ability.abilityLogic != null)
            {
                // 参照(ability)をそのままAddするのではなく、
                // 新しいKnifeAbilityインスタンスを生成してAddする
                KnifeAbility newAbility = new KnifeAbility(
                    UnityEngine.Object.Instantiate(ability.abilityLogic),
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
        _name = data._name;
        rank = data.rank;
        element = data.element;
        power = data.power;
        elementPower = data.elementPower;
        count_Multiple = data.count_Multiple;
        prefab = data.prefab;
        hitEffect = data.hitEffect;
        flavortext = data.flavortext;

        foreach (var ability in data.abilities)
        {
            if (ability.abilityLogic != null)
            {
                // 参照(ability)をそのままAddするのではなく、
                // 新しいKnifeAbilityインスタンスを生成してAddする
                KnifeAbility newAbility = new KnifeAbility(
                    UnityEngine.Object.Instantiate(ability.abilityLogic),
                    ability.effectID
                );
                abilities.Add(newAbility);
            }
        }
    }
}
