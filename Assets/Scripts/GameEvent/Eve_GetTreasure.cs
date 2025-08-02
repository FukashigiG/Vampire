using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eve_GetTreasure : Base_EventCtrler
{
    [SerializeField] GameObject treasureButtonObj;

    [SerializeField] Transform buttonArea;

    [SerializeField] List<Base_TreasureData> treasures;

    private void OnEnable()
    {
        int num_Options = Random.Range(3, 5);

        // 秘宝のリストのコピーを作成
        List<Base_TreasureData> copyList = new List<Base_TreasureData>(treasures);

        for (int i = 0; i < num_Options; i++)
        {
            // 万が一リストの中身が空なら中断
            if (copyList.Count <= 0) break;

            //選択肢となる秘宝を決定
            Base_TreasureData y = copyList[Random.Range(0, copyList.Count)];

            // ボタンを指定のTransform下に生成
            var x = Instantiate(treasureButtonObj, buttonArea);

            // ボタンを選ばれた秘宝で初期化
            x.GetComponent<Button_Treasure>().Initialize(y);

            // コピーされたリストから選ばれたものを削除
            // 重複を防ぐため
            copyList.Remove(y);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
