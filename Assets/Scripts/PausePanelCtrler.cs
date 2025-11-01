using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Linq;
using UniRx;

public class PausePanelCtrler : MonoBehaviour
{
    // �ꗗ�ƂȂ�I�u�W�F�N�g��Transform
    // Instantiate���ɐe�̈����Ƃ��ēn��
    [SerializeField] Transform knifeArea;
    [SerializeField] Transform treasureArea;

    // ���������
    [SerializeField] GameObject knifeImagePrefab;
    [SerializeField] GameObject treasureImagePrefab;

    PlayerInventory playerInventory;

    // �C�x���g�w�ǂ��܂Ƃ߂ĊǗ����邽�߂�Disposable
    private CompositeDisposable _disposables = new CompositeDisposable();

    // �����i�C�t/��󗓂ɐ������ꂽ�I�u�W�F�N�g���Ǘ����邽�߂̂���
    Dictionary<KnifeData_RunTime, GameObject> knifeImageDictionaty = new();
    Dictionary<Base_TreasureData, GameObject> treasureImageDictionaty = new();

    private void Awake()
    {
        // �������A�N�e�B�u�łȂ��I�u�W�F�N�g�ւ̃A�^�b�`��z��̂��߁AAwake��Start�͏�肭���삵�Ȃ�
    }

    // ������
    public void Initialize(InputAction action)
    {
        action.performed += TogglePanel; // �����œn���ꂽ�A�N�V���������s���ꂽ��TogglePanel���Ă�

        playerInventory = PlayerController.Instance._status.inventory; // PlayerInventory���v���C���[����擾

        // --- �i�C�t�̍w�ǐݒ� ---
        playerInventory.runtimeKnives
            .ObserveAdd()
            .Subscribe(e => OnKnifeAdded(e.Value))
            .AddTo(_disposables);

        playerInventory.runtimeKnives
            .ObserveRemove()
            .Subscribe(e => OnKnifeRemoved(e.Value))
            .AddTo(_disposables);

        // --- ���̍w�ǐݒ� ---
        playerInventory.runtimeTreasure
            .ObserveAdd()
            .Subscribe(e => OnTreasureAdded(e.Value))
            .AddTo(_disposables);

        playerInventory.runtimeTreasure
            .ObserveRemove()
            .Subscribe(e => OnTreasureRemoved(e.Value))
            .AddTo(_disposables);

        Debug.Log("set toggle");

        // �ŏ��ɁA���ɓo�^����Ă��镨�ɂ��āA�ǉ�����������
        foreach (var knives in playerInventory.runtimeKnives) OnKnifeAdded(knives);
        foreach (var treasures in playerInventory.runtimeTreasure) OnTreasureAdded(treasures);
    }

    // �i�C�t���ǉ����ꂽ��
    void OnKnifeAdded(KnifeData_RunTime knifeData)
    {
        // �i�C�t�ꗗ�ɐV���ɐ���
        var imageObj = Instantiate(knifeImagePrefab, knifeArea);
        imageObj.GetComponent<Image>().sprite = knifeData.sprite;

        // �����ɓo�^
        knifeImageDictionaty[knifeData] = imageObj;
    }

    // �i�C�t���폜���ꂽ��
    void OnKnifeRemoved(KnifeData_RunTime knifeData)
    {
        // �����ɓo�^����Ă�����̂ł����
        if (knifeImageDictionaty.TryGetValue(knifeData, out var obj))
        {
            // �v�f���폜
            Destroy(obj);
            knifeImageDictionaty.Remove(knifeData);
        }
    }

    // ��󂪒ǉ����ꂽ��
    void OnTreasureAdded(Base_TreasureData treasureData)
    {
        var imageObj = Instantiate(treasureImagePrefab, treasureArea);
        imageObj.GetComponent<Image>().sprite = treasureData.icon;

        treasureImageDictionaty[treasureData] = imageObj;
    }

    // ��󂪍폜���ꂽ��
    void OnTreasureRemoved(Base_TreasureData treasureData)
    {
        if (treasureImageDictionaty.TryGetValue(treasureData, out var obj))
        {
            Destroy(obj);
            treasureImageDictionaty.Remove(treasureData);
        }
    }

    // �p�l���\��
    void TogglePanel(InputAction.CallbackContext context)
    {
        this.gameObject.SetActive(true);
    }

    // �p�l����\��
    public void CloseThis()
    {
        this.gameObject.SetActive(false);
    }

    // ���̃I�u�W�F�N�g���j�������Ƃ��ɁA�w�ǂ����ׂĉ���
    private void OnDestroy()
    {
        _disposables.Dispose();
    }

    // ��\���ɂȂ����Ƃ�
    private void OnDisable()
    {
        
    }
}
