using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class CharaSelect_Button : MonoBehaviour
{
    [SerializeField] Text txt_Btn;

    Subject<PlayerCharaData> subject_OnClicked = new Subject<PlayerCharaData>();
    public IObservable<PlayerCharaData> _onClicked => subject_OnClicked;

    public void Setup(PlayerCharaData chara)
    {
        txt_Btn.text = chara._name;

        GetComponent<Button>().onClick.AddListener(() =>
        {
            subject_OnClicked.OnNext(chara);
        });
    }
}
