using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Button_Treasure : MonoBehaviour
{
    [SerializeField] Image image_Sprite;

    //[SerializeField] GameObject detailWindow;

    [SerializeField] Button button;

    Animator _animator;

    Base_TreasureData treasureData;

    //�����ꂽ�Ƃ��ɔ�΂��ʒm
    //�Q�[���I�����ɂ�GameAdmin��Dispose�����
    public static Subject<Base_TreasureData> clicked { get; private set; } = new Subject<Base_TreasureData>();

    // �J�[�\�����d�Ȃ����A�O�ꂽ���΂��ʒm
    public Subject<Base_TreasureData> pointerEntered { get; private set; } = new Subject<Base_TreasureData>();
    public Subject<Base_TreasureData> pointerExited { get; private set; } = new Subject<Base_TreasureData>();

    void Start()
    {
        _animator = GetComponent<Animator>();

        button.onClick.AddListener(() => clicked.OnNext(treasureData));
    }

    // ������
    public void Initialize(Base_TreasureData x)
    {
        treasureData = x;

        image_Sprite.sprite = treasureData.icon;

        //detailWindow.GetComponent<AKBtn_Detail>().Initialize(knifeData);

        //detailWindow.SetActive(false);
    }

    // �J�[�\�����������Ƃ�
    public void OnPointerEnter(PointerEventData data)
    {
        // �A�j���[�V����
        _animator?.SetBool("highlighted", true);

        // �ڍ׃E�C���h�E�̕\��
        //detailWindow.SetActive(true);

        // �q�G�����L�[�ōŉ��Ɉړ�����
        // ���O�ʂɕ\�������
        //transform.SetAsLastSibling();

        pointerEntered.OnNext(treasureData);
    }

    // �J�[�\�����O�ꂽ�Ƃ�
    public void OnPointerExit(PointerEventData data)
    {
        _animator?.SetBool("highlighted", false);

        //detailWindow.SetActive(false);

        pointerExited.OnNext(treasureData);
    }

    private void OnDestroy()
    {
        pointerEntered.Dispose();
        pointerExited.Dispose();
    }
}
