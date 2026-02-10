using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class CharaSelect_Button : MonoBehaviour
{
    [SerializeField] Text txt_CharaName;
    [SerializeField] Text txt_Element;

    Subject<PlayerCharaData> subject_OnClicked = new Subject<PlayerCharaData>();
    public IObservable<PlayerCharaData> _onClicked => subject_OnClicked;

    public void Setup(PlayerCharaData chara)
    {
        txt_CharaName.text = chara._name;

        switch (chara.masteredElement)
        {
            case Element.White:
                txt_Element.text = "îí";
                break;

            case Element.Blue:
                txt_Element.text = "ê¬";
                break;

            case Element.Red:
                txt_Element.text = "ê‘";
                break;

            case Element.Yellow:
                txt_Element.text = "â©";
                break;

            default:
                txt_Element.text = "îí";
                break;
        }

        GetComponent<Button>().onClick.AddListener(() =>
        {
            subject_OnClicked.OnNext(chara);
        });
    }
}
