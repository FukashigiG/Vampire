using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UniRx;

public class Button_AddKnifeCtrler : MonoBehaviour
{
    [SerializeField] Image image_Sprite;

    [SerializeField] Button button;

    Animator _animator;

    KnifeData knifeData;

    //�����ꂽ�Ƃ��ɔ�΂��ʒm
    //�Q�[���I�����ɂ�GameAdmin��Dispose�����
    public static Subject<KnifeData> clicked { get; private set; } = new Subject<KnifeData>();

    void Start()
    {
        _animator = GetComponent<Animator>();

        button.onClick.AddListener(() => clicked.OnNext(knifeData));
    }

    // ������
    public void Initialize(KnifeData x)
    {
        knifeData = x;

        image_Sprite.sprite = knifeData.sprite;
    }

    // �J�[�\�����������Ƃ�
    public void OnPointerEnter(PointerEventData data)
    {
        _animator?.SetBool("highlighted", true);

        Debug.Log("dfd");
    }

    // �J�[�\�����O�ꂽ�Ƃ�
    public void OnPointerExit(PointerEventData data)
    {
        _animator?.SetBool("highlighted", true);
    }
}
