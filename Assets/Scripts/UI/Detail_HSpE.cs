using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Detail_HSpE : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject detailWindow;

    [SerializeField] Text txt_Name;
    [SerializeField] Text txt_Name_DetailWindow;
    [SerializeField] Text txt_Description;

    BaseHSpE hspe;

    public void Initialize(BaseHSpE _hspe)
    {
        hspe = _hspe;

        txt_Name.text = hspe.effectName;
        txt_Name_DetailWindow.text = hspe.effectName;
        txt_Description.text = hspe.description;

        detailWindow.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData data)
    {
        // �ڍ׃E�C���h�E�̕\��
        detailWindow.SetActive(true);
    }

    // �J�[�\�����O�ꂽ�Ƃ�
    public void OnPointerExit(PointerEventData data)
    {
        detailWindow.SetActive(false);
    }
}
