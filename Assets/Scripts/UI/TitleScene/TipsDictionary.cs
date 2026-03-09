using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine.UI;
using TMPro;


public class TipsDictionary : SingletonMono<TipsDictionary>
{
    [SerializeField] GameObject prefab_Btn;

    [SerializeField] GameObject bodyPanel;

    [SerializeField] Button btn_Close;
    [SerializeField] Transform btnArea;
    [SerializeField] TextMeshProUGUI txt_Title;
    [SerializeField] TextMeshProUGUI txt_Discription;
    [SerializeField] List<Icon_ESA> icons_ESA;

    List<Tips> tips = new List<Tips>();

    public void Initialize(Button btn_Open)
    {
        // 画面開閉登録
        btn_Open.onClick.AddListener(OpenPanel);
        btn_Close.onClick.AddListener(ClosePanel);

        // 全データ取得
        tips = Resources.LoadAll<Tips>("GameDatas/Tips").ToList();

        // それぞれのデータの分だけボタンを用意、初期化
        foreach (Tips tp in tips)
        {
            var obj =  Instantiate(prefab_Btn, btnArea);

            Button btn = obj.GetComponent<Button>();

            btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =tp.Title;

            btn.onClick.AddListener(() =>
            {
                SetInfo(tp);
            });
        }

    }

    // 渡された情報を表示
    void SetInfo(Tips tp)
    {
        txt_Title .text = tp.Title;

        txt_Discription.text = tp.txt;
    }

    void OpenPanel()
    {
        bodyPanel.SetActive(true);

        txt_Title.text = "Tips";
        txt_Discription.text = "";
    }

    void ClosePanel()
    {
        bodyPanel.SetActive(false);
    }
}
