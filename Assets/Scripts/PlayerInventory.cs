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
    public List<Base_TreasureData> runtimeTreasure { get; private set; } = new List<Base_TreasureData>();

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
        runtimeKnives.Add(x);
    }

    // ������肷�鏈��
    public void AddTreasure(Base_TreasureData x)
    {
        //if (runtimeTreasure.Contains(x)) return; // �����A�C�e���͒ǉ����Ȃ�

        runtimeTreasure.Add(x);

        x.OnAdd(status);

        // ���̔��ɑ΂���CompositeDisposable��V�K�쐬
        var disposables = new CompositeDisposable();
        _itemDisposables.Add(x, disposables);

        x.SubscribeToEvent(status, disposables);

        Debug.Log($"{x._name} ���擾�����I");
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
