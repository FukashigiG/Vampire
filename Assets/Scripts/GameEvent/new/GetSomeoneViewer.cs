using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GetSomeoneViewer : SingletonMono<GetSomeoneViewer>
{
    // 武器・秘宝獲得発動時、その内容を表示するUI用スクリプト

    [field: SerializeField] public GameObject body_Panel { get; private set; }

    [SerializeField] Button[] buttons;

    enum itemType { knife, treasure}
    [Serializable] class TypeAndWeight { public itemType itemType; public int weight;}

    [SerializeField] TypeAndWeight[] typeAndWeight;

    public void ShowEvent()
    {
        body_Panel.SetActive(true);

        foreach(var button  in buttons)
        {
            button.onClick.AddListener(() =>
            {
                ClosePanel();
            });
        }
    }

    void ClosePanel()
    {
        foreach(var button in buttons)
        {
            button.onClick.RemoveAllListeners();
        }

        body_Panel.SetActive(false);
    }
}
