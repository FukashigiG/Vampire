using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class Eve_GetKnife : Base_EventCtrler
{
    [SerializeField] GameObject button_Option;
    [SerializeField] AKBtn_Detail window_ShowDiscription;

    [SerializeField] Button button_reroll;

    [SerializeField] List<KnifeData> allKnifeData;

    GameObject discriptionTarget;

    public override void Initialize()
    {
        base.Initialize();

        button_reroll.onClick.AddListener(() =>
        {
            Reroll();
        });
    }

    //パネルがActiveになったとき
    protected override void OnEnable()
    {
        base.OnEnable();

        GenerateButtons();
    }

    void GenerateButtons()
    {
        int num_Option = Random.Range(2, 6);

        //２～５個のボタンを用意
        for (int i = 0; i < num_Option; i++)
        {
            //ランダムなナイフを選出
            var randomKnife = DrawingKnives();

            //生成したボタンをbuttonObjと置く
            var buttonObj = Instantiate(button_Option, buttonArea);

            //コンポーネントの取得
            var buttonCtrler = buttonObj.GetComponent<Button_Knife>();

            //ボタンに選出したナイフの情報を渡す
            buttonCtrler.Initialize(randomKnife);

            //ボタンが押された際に、それを検知し関数を実行
            buttonCtrler.clicked.Subscribe(xx => Choice(xx)).AddTo(buttonCtrler);

            

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

            var buttonRect = buttonObj.GetComponent<RectTransform>();
            buttonRect.anchoredPosition = new Vector2(y * i - x / 2, 0);

            // ボタンにカーソルが在ったときの通知に購読
            buttonCtrler.pointerEntered.Subscribe(x =>
            {
                window_ShowDiscription.gameObject.SetActive(true);
                window_ShowDiscription.Initialize(x.knifeData);
                discriptionTarget = x._object;
                window_ShowDiscription.GetComponent<RectTransform>().anchoredPosition = buttonRect.anchoredPosition + new Vector2(240, 0);

            }).AddTo(buttonCtrler);

            buttonCtrler.pointerExited.Subscribe(x =>
            {
                if(discriptionTarget == x._object) window_ShowDiscription.gameObject.SetActive(false);

            }).AddTo(buttonCtrler);
        }
    }

    void Choice(KnifeData_RunTime knifeData)
    {
        //プレイヤーに抽選されたナイフの追加
        PlayerController.Instance._status.inventory.AddKnife(knifeData);

        //パネルを閉じる
        this.gameObject.SetActive(false);
    }

    //ナイフの抽選
    KnifeData_RunTime DrawingKnives()
    {
        int x = Random.Range(0, allKnifeData.Count);

        var y = new KnifeData_RunTime(allKnifeData[x]);

        return y;
    }

    void Reroll()
    {
        DisposeButtons();

        GenerateButtons();
    }
}
