using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Base_EventCtrler : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;

    [SerializeField] Image EventSpriteArea;
    [SerializeField] protected Transform buttonArea;

    [SerializeField] protected Button button_Skip;

    public virtual void Initialize()
    {
        // �X�L�b�v�{�^�����������̂ɔ������āA�p�l�������悤��
        button_Skip?.onClick.AddListener(() => this.gameObject.SetActive(false));
    }

    protected virtual void OnEnable()
    {

    }

    // ���̃p�l��������Ƃ��iactiveSelf��false�ɂȂ�Ƃ��j
    protected void OnDisable()
    {
        //buttonArea�̎q�I�u�W�F�N�g��S�폜
        foreach (Transform button in buttonArea)
        {
            Destroy(button.gameObject);
        }
    }
}
