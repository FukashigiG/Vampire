using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharaSelect_Button : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txt_CharaName;
    [SerializeField] TextMeshProUGUI txt_Element;

    Subject<PlayerCharaData> subject_OnClicked = new Subject<PlayerCharaData>();
    public IObservable<PlayerCharaData> _onClicked => subject_OnClicked;

    public void Setup(PlayerCharaData chara)
    {
        txt_CharaName.text = chara._name;

        switch (chara.masteredElement)
        {
            case Element.White:
                txt_Element.text = "白";
                txt_Element.color = Color.white;
                break;

            case Element.Blue:
                txt_Element.text = "青";
                txt_Element.color = Color.blue;
                break;

            case Element.Red:
                txt_Element.text = "赤";
                txt_Element.color = Color.red;
                break;

            case Element.Yellow:
                txt_Element.text = "黄";
                txt_Element.color = Color.yellow;
                break;

            default:
                txt_Element.text = "白";
                txt_Element.color = Color.white;
                break;
        }

        GetComponent<Button>().onClick.AddListener(() =>
        {
            subject_OnClicked.OnNext(chara);
        });
    }
}
