using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

[RequireComponent(typeof(PlayerStatus))]
public class PlayerInventory : MonoBehaviour
{
    public List<KnifeData> runtimeKnives { get; private set; } = new List<KnifeData>();
    public List<Base_TreasureData> runtimeTreasure { get; private set; } = new List<Base_TreasureData>();

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
        x.SubscribeToEvent();

        Debug.Log($"{x._name} ���擾�����I");
    }
}
