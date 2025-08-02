using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using static UnityEditor.Progress;

[RequireComponent(typeof(PlayerStatus))]
public class PlayerInventory : MonoBehaviour
{
    public ReactiveCollection<KnifeData> runtimeKnives { get; private set; } = new ReactiveCollection<KnifeData>();
    public ReactiveCollection<Base_TreasureData> runtimeTreasure { get; private set; } = new();

    // �A�C�e���Ƃ��̃C�x���g�w�ǂ�R�t���ĊǗ�����
    private readonly Dictionary<Base_TreasureData, CompositeDisposable> _itemDisposables = new Dictionary<Base_TreasureData, CompositeDisposable>();


    PlayerStatus status;

    private void Awake()
    {
        status = GetComponent<PlayerStatus>();
    }

    void Start()
    {

    }

    // �i�C�t����肷�鏈��
    public void AddKnife(KnifeData x)
    {
        // �n���ꂽ�f�[�^�̃C���X�^���X�𐶐�
        // �������邱�ƂŁA���ナ�X�g���̃i�C�t�f�[�^��ҏW����ہA���f�[�^�ƂȂ�X�N���v�^�u���I�u�W�F�N�g�A�Z�b�g�̐��l�������炸�ɍς�
        var y = Instantiate(x);

        // ���X�g�ɉ�����
        runtimeKnives.Add(y);
    }

    // ������肷�鏈��
    public void AddTreasure(Base_TreasureData x)
    {
        //if (runtimeTreasure.Contains(x)) return; // �����A�C�e���͒ǉ����Ȃ�

        // �n���ꂽ�f�[�^�̃C���X�^���X�𐶐�
        var y = Instantiate(x);

        // ���X�g�ɉ�����
        runtimeTreasure.Add(y);

        // ���́A���莞�̏��������s
        y.OnAdd(status);

        // ���̔��ɑ΂���CompositeDisposable��V�K�쐬
        var disposables = new CompositeDisposable();
        _itemDisposables.Add(y, disposables);

        y.SubscribeToEvent(status, disposables);

        Debug.Log($"{y._name} ���擾�����I");
    }

    public void RemoveTreasure(Base_TreasureData x)
    {
        if(! runtimeTreasure.Contains(x)) return; // �������̔��������ĂȂ��Ȃ疳��

        // ���̃C�x���g�w�ǂ�j��
        if(_itemDisposables.TryGetValue(x, out var disposables))
        {
            disposables.Dispose();
            _itemDisposables.Remove(x);
        }

        // �����Ƃ��̏��������s
        x.OnRemove(status);

        // ������󃊃X�g���珜�O
        runtimeTreasure.Remove(x);

        Debug.Log($"{x._name} ���������I");
    }

    private void OnDestroy()
    {
        foreach( var x in _itemDisposables.Values )
        {
            x.Dispose();
        }

        _itemDisposables.Clear();
    }
}
