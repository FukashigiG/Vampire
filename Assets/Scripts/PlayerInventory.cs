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

    // ナイフを入手する処理
    public void AddKnife(KnifeData x)
    {
        runtimeKnives.Add(x);
    }

    // 秘宝を入手する処理
    public void AddTreasure(Base_TreasureData x)
    {
        //if (runtimeTreasure.Contains(x)) return; // 同じアイテムは追加しない

        runtimeTreasure.Add(x);

        x.OnAdd(status);
        x.SubscribeToEvent();

        Debug.Log($"{x._name} を取得した！");
    }
}
