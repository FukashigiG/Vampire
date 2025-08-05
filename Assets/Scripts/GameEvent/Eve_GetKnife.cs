using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class Eve_GetKnife : Base_EventCtrler
{
    [SerializeField] GameObject button_Option;

    [SerializeField] List<KnifeData> allKnifeData;

    public override void Initialize()
    {
        base.Initialize();

        //ナイフ追加画面のボタンが押された際に、それを検知し関数を実行
        Button_Knife.clicked.Subscribe(xx => Choice(xx)).AddTo(this);
    }

    //パネルがActiveになったとき
    protected override void OnEnable()
    {
        base.OnEnable();

        int num_Option = Random.Range(2, 6);

        //２～５個のボタンを用意
        for (int i = 0; i < num_Option; i++)
        {
            //生成したボタンをbuttonObjと置く
            var buttonObj =  Instantiate(button_Option, buttonArea);

            //ランダムなナイフを選出
            var randomKnife = DrawingKnives();

            //コンポーネントの取得
            var buttonCtrler = buttonObj.GetComponent<Button_Knife>();

            //ボタンに選出したナイフの情報を渡す
            buttonCtrler.Initialize(randomKnife);

            //ボタンが押された際の処理はStart()にて記載済み

            // 位置を変更

            float x;

            switch (num_Option)
            {
                case 2:
                    x = 500;
                    break;

                case 3:
                    x = 650;
                    break;

                default:
                    x = 800;
                    break;
            }

            float y = x / (num_Option - 1);

            buttonObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(y * i - x / 2, 0);
        }
    }

    void Choice(KnifeData knifeData)
    {
        //プレイヤーに抽選されたナイフの追加
        PlayerController.Instance._status.inventory.AddKnife(knifeData);

        //パネルを閉じる
        this.gameObject.SetActive(false);
    }

    //ナイフの抽選
    KnifeData DrawingKnives()
    {
        int x = Random.Range(0, allKnifeData.Count);

        var y = allKnifeData[x];

        return y;
    }
}
