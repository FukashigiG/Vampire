using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class Button_AddKnifeCtrler : MonoBehaviour
{
    [SerializeField] Text txt_Name;
    [SerializeField] Image image_Sprite;

    [SerializeField] Button button;

    KnifeData knifeData;

    //�����ꂽ�Ƃ��ɔ�΂��ʒm
    //�Q�[���I�����ɂ�GameAdmin��Dispose�����
    public static Subject<KnifeData> clicked { get; private set; } = new Subject<KnifeData>();

    void Start()
    {
        button.onClick.AddListener(() => clicked.OnNext(knifeData));
    }

    public void SetInfo(KnifeData x)
    {
        knifeData = x;

        txt_Name.text = knifeData.name;
        image_Sprite.sprite = knifeData.sprite;
    }
}
