using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[RequireComponent(typeof(PlayerStatus))]
public class PlayerInventory : MonoBehaviour
{
    public ReactiveCollection<KnifeData_RunTime> runtimeKnives { get; private set; } = new ReactiveCollection<KnifeData_RunTime>();
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
    public void AddKnife(KnifeData_RunTime x)
    {
        // �n���ꂽ�i�C�t�̃f�[�^�����ɏ��������������^�C���p�i�C�t�f�[�^
        var y = new KnifeData_RunTime(x);

        // ���X�g�ɉ�����
        runtimeKnives.Add(y);
    }

    public void RemoveKnife(KnifeData_RunTime x)
    {
        if (! runtimeKnives.Contains(x)) return;

        runtimeKnives.Remove(x);
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
