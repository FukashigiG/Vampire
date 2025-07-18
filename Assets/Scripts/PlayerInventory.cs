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

    // アイテムとそのイベント購読を紐付けて管理する
    private readonly Dictionary<Base_TreasureData, CompositeDisposable> _itemDisposables = new Dictionary<Base_TreasureData, CompositeDisposable>();


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

        // この秘宝に対するCompositeDisposableを新規作成
        var disposables = new CompositeDisposable();
        _itemDisposables.Add(x, disposables);

        x.SubscribeToEvent(status, disposables);

        Debug.Log($"{x._name} を取得した！");
    }

    public void RemoveTreasure(Base_TreasureData x)
    {
        if(! runtimeTreasure.Contains(x)) return; // もしその秘宝を持ってないなら無視

        // 秘宝のイベント購読を破壊
        if(_itemDisposables.TryGetValue(x, out var disposables))
        {
            disposables.Dispose();
            _itemDisposables.Remove(x);
        }

        // 失うときの処理を実行
        x.OnRemove(status);

        // 所持秘宝リストから除外
        runtimeTreasure.Remove(x);

        Debug.Log($"{x._name} を失った！");
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
