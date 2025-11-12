using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

// これ↓を記述しておくことで、SystemとUnityEngineでRanbomが競合しなくなる
using Random = UnityEngine.Random;
using Cysharp.Threading.Tasks;

public class Eve_GetTreasure : Base_EventCtrler
{
    [SerializeField] GameObject treasureButtonObj;

    List<Base_TreasureData> allTreasures;

    PlayerInventory playerInventory;

    public override void Initialize()
    {
        base.Initialize();

        playerInventory = PlayerController.Instance._status.inventory;

        // ゲームに登録された全ての秘宝を配列として取得
        var x = Resources.LoadAll<Base_TreasureData>("GameDatas/Treasure");

        // 配列をListに変換、treasuresに格納
        allTreasures = new List<Base_TreasureData>(x);
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        // PlayerInventoryの持つ秘宝のIDをHashSetに格納
        // HashSetを使うことで、後の「含まれてるか」のチェックが楽になる
        HashSet<string> excludedIDs = new HashSet<string>(playerInventory.runtimeTreasure.Select(x => x._name));

        // Playerの持ってない秘宝のリストを作成
        List<Base_TreasureData> availableTreasures = allTreasures
            .Where(treasureAsset => !excludedIDs.Contains(treasureAsset._name))
            .ToList();

        // リストの中身をシャッフル
        availableTreasures = availableTreasures.OrderBy(a => Guid.NewGuid()).ToList();

        // 選択肢の総数を決定
        int num_Options = Random.Range(3, 5);

        for (int i = 0; i < num_Options; i++)
        {
            // 万が一リストの中身が空なら中断
            if (availableTreasures.Count <= 0) break;

            //選択肢となる秘宝を決定
            Base_TreasureData treasureData = availableTreasures[0];

            // ボタンを指定のTransform下に生成
            var buttonObj = Instantiate(treasureButtonObj, buttonArea);

            // ボタンを選ばれた秘宝で初期化
            buttonObj.GetComponent<Button_Treasure>().Initialize(treasureData);

            // 生成したボタンが押されたときの処理を追加
            buttonObj.GetComponent<Button>().onClick.AddListener(() =>
            {
                // 選択肢が選ばれた
                Choice(treasureData);
            });

            // コピーされたリストから選ばれたものを削除
            // 重複を防ぐため
            availableTreasures.Remove(treasureData);
        }
    }

    void Choice(Base_TreasureData treasureData)
    {
        //　選択された秘宝をインベントリに追加
        playerInventory.AddTreasure(treasureData);

        //パネルを閉じる
        this.gameObject.SetActive(false);
    }
}
