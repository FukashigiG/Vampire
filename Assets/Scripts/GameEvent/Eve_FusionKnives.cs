using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Eve_FusionKnives : Base_EventCtrler
{
    PlayerInventory inventory;

    [SerializeField] GameObject button_Knife;

    public override void Initialize()
    {
        base.Initialize();

        inventory = PlayerController.Instance._status.inventory;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        // 手持ちナイフ全てのボタンを生成
        foreach(var knife in inventory.runtimeKnives)
        {
            // ボタンを生成
            GameObject button = Instantiate(button_Knife, buttonArea);

            // ボタンにナイフの情報を渡して初期化
            button.GetComponent<Button_Knife>().Initialize(knife);

            // ボタンが押されたときの処理を登録
            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                Clicked(knife);
            });
        }
    }

    // ボタンが押された時の処理
    void Clicked(KnifeData knifeData)
    {

    }
}
