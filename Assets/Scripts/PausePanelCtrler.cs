using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Linq;

public class PausePanelCtrler : MonoBehaviour
{
    [SerializeField] Transform knifeArea;

    [SerializeField] GameObject knifeButtonPrefab;

    [SerializeField] GameObject detailWindow;

    private void Awake()
    {
        // �������A�N�e�B�u�łȂ��I�u�W�F�N�g�ւ̃A�^�b�`��z��̂��߁AAwake��Start�͏�肭���삵�Ȃ�
    }

    // ������
    public void Initialize(InputAction action)
    {
        action.performed += TogglePanel; // �����œn���ꂽ�A�N�V���������s���ꂽ��TogglePanel���Ă�

        Debug.Log("set toggle");
    }

    // �p�l���\��
    void TogglePanel(InputAction.CallbackContext context)
    {
        this.gameObject.SetActive(true);
    }

    // �\�����ꂽ�Ƃ�
    private void OnEnable()
    {
        List<KnifeData> knives = PlayerController.Instance._status.inventory.runtimeKnives.ToList();

        foreach(var knifeData in knives)
        {
            var x = Instantiate(knifeButtonPrefab, knifeArea);

            x.GetComponent<Button_AddKnifeCtrler>().Initialize(knifeData);
        }
    }

    // �p�l����\��
    public void CloseThis()
    {
        // �S�폜
        foreach(Transform button in knifeArea)
        {
            Destroy(button.gameObject);
        }

        this.gameObject.SetActive(false);
    }

    // ��\���ɂȂ����Ƃ�
    private void OnDisable()
    {
        
    }
}
