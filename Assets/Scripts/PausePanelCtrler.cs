using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Linq;
using UniRx;

public class PausePanelCtrler : MonoBehaviour
{
    [SerializeField] Transform knifeArea;
    [SerializeField] Transform treasureArea;

    [SerializeField] GameObject knifeImagePrefab;
    [SerializeField] GameObject treasureImagePrefab;

    [SerializeField] GameObject detailWindow;

    PlayerInventory playerInventory;

    // �C�x���g�w�ǂ��܂Ƃ߂ĊǗ����邽�߂�Disposable
    private CompositeDisposable _disposables = new CompositeDisposable();

    Dictionary<KnifeData, GameObject> knifeImageDictionaty = new();
    Dictionary<Base_TreasureData, GameObject> treasureImageDictionaty = new();

    private void Awake()
    {
        // �������A�N�e�B�u�łȂ��I�u�W�F�N�g�ւ̃A�^�b�`��z��̂��߁AAwake��Start�͏�肭���삵�Ȃ�
    }

    // ������
    public void Initialize(InputAction action)
    {
        action.performed += TogglePanel; // �����œn���ꂽ�A�N�V���������s���ꂽ��TogglePanel���Ă�

        playerInventory = PlayerController.Instance._status.inventory;

        // --- �i�C�t�̍w�ǐݒ� ---
        playerInventory.runtimeKnives
            .ObserveAdd()
            .Subscribe(e => OnKnifeAdded(e.Value))
            .AddTo(_disposables);

        playerInventory.runtimeKnives
            .ObserveRemove()
            .Subscribe(e => OnKnifeRemoved(e.Value))
            .AddTo(_disposables);

        // --- �g���W���[�̍w�ǐݒ� ---
        playerInventory.runtimeTreasure
            .ObserveAdd()
            .Subscribe(e => OnTreasureAdded(e.Value))
            .AddTo(_disposables);

        playerInventory.runtimeTreasure
            .ObserveRemove()
            .Subscribe(e => OnTreasureRemoved(e.Value))
            .AddTo(_disposables);

        Debug.Log("set toggle");

        // �ŏ��ɁA���ɓo�^����Ă��镨�ɂ��Ēǉ�����������
        foreach (var knives in playerInventory.runtimeKnives) OnKnifeAdded(knives);
        foreach (var treasures in playerInventory.runtimeTreasure) OnTreasureAdded(treasures);
    }

    void OnKnifeAdded(KnifeData knifeData)
    {
        var imageObj = Instantiate(knifeImagePrefab, knifeArea);
        imageObj.GetComponent<Image>().sprite = knifeData.sprite;

        knifeImageDictionaty[knifeData] = imageObj;
    }

    void OnKnifeRemoved(KnifeData knifeData)
    {
        if (knifeImageDictionaty.TryGetValue(knifeData, out var obj))
        {
            Destroy(obj);
            knifeImageDictionaty.Remove(knifeData);
        }
    }


    void OnTreasureAdded(Base_TreasureData treasureData)
    {
        var imageObj = Instantiate(knifeImagePrefab, knifeArea);
        imageObj.GetComponent<Image>().sprite = treasureData.icon;

        treasureImageDictionaty[treasureData] = imageObj;
    }

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

    /*
    // �\�����ꂽ�Ƃ�
    private void OnEnable()
    {
        List<KnifeData> knives = inventory.runtimeKnives.ToList();

        foreach(var knifeData in knives)
        {
            var x = Instantiate(knifeButtonPrefab, knifeArea);

            x.GetComponent<Button_AddKnifeCtrler>().Initialize(knifeData);
        }
    }
    */

    // �p�l����\��
    public void CloseThis()
    {
        this.gameObject.SetActive(false);
    }

    // ���̃I�u�W�F�N�g���j�������Ƃ��ɍw�ǂ����ׂĉ���
    private void OnDestroy()
    {
        _disposables.Dispose();
    }

    // ��\���ɂȂ����Ƃ�
    private void OnDisable()
    {
        
    }
}
