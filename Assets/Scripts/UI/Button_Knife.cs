using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UniRx;

public class Button_Knife : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image image_Sprite;

    //[SerializeField] GameObject detailWindow;

    [SerializeField] Button button;

    Animator _animator;

    KnifeData knifeData;
 
    //�����ꂽ�Ƃ��ɔ�΂��ʒm
    //�Q�[���I�����ɂ�GameAdmin��Dispose�����
    public static Subject<KnifeData> clicked { get; private set; } = new Subject<KnifeData>();

    // �J�[�\�����d�Ȃ����A�O�ꂽ���΂��ʒm
    public Subject<KnifeData> pointerEntered { get; private set; } = new Subject<KnifeData> ();
    public Subject<KnifeData> pointerExited { get; private set; } = new Subject<KnifeData> ();

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

        pointerEntered.OnNext(knifeData);
    }

    // �J�[�\�����O�ꂽ�Ƃ�
    public void OnPointerExit(PointerEventData data)
    {
        _animator?.SetBool("highlighted", false);

        //detailWindow.SetActive(false);

        pointerExited.OnNext(knifeData);
    }

    private void OnDestroy()
    {
        pointerEntered.Dispose();
        pointerExited.Dispose();
    }
}
