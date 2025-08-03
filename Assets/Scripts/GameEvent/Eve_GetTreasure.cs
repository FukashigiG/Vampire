using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

// これ↓を記述しておくことで、SystemとUnityEngineでRanbomが競合しなくなる
using Random = UnityEngine.Random;

public class Eve_GetTreasure : Base_EventCtrler
{
    [SerializeField] GameObject treasureButtonObj;

    [SerializeField] Transform buttonArea;

    List<Base_TreasureData> allTreasures;

    public override void Initialize()
    {
        base.Initialize();

        // ゲームに登録された全ての秘宝を配列として取得
        var x = Resources.LoadAll<Base_TreasureData>("GameDatas/Treasure");

        // 配列をListに変換、treasuresに格納
        allTreasures = new List<Base_TreasureData>(x);
    }

    private void OnEnable()
    {
        // PlayerInventoryの持つ秘宝のIDをHashSetに格納
        // HashSetを使うことで、後の「含まれてるか」のチェックが楽になる
        HashSet<int> excludedIDs = new HashSet<int>();

        foreach (var treasure in PlayerController.Instance._status.inventory.runtimeTreasure)
        {
            excludedIDs.Add(treasure.uniqueIP);
        }

        // Playerの持ってない秘宝のリストを作成
        List<Base_TreasureData> availableTreasures = allTreasures
            .Where(treasureAsset => !excludedIDs.Contains(treasureAsset.uniqueIP))
            .ToList();

        // リストの中身をシャッフル
        availableTreasures = availableTreasures.OrderBy(a => Guid.NewGuid()).ToList();

        int num_Options = Random.Range(3, 5);

        for (int i = 0; i < num_Options; i++)
        {
            // 万が一リストの中身が空なら中断
            if (availableTreasures.Count <= 0) break;

            //選択肢となる秘宝を決定
            Base_TreasureData y = availableTreasures[0];

            // ボタンを指定のTransform下に生成
            var x = Instantiate(treasureButtonObj, buttonArea);

            // ボタンを選ばれた秘宝で初期化
            x.GetComponent<Button_Treasure>().Initialize(y);

            // コピーされたリストから選ばれたものを削除
            // 重複を防ぐため
            availableTreasures.Remove(y);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
