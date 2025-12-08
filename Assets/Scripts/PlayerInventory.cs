using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

[RequireComponent(typeof(PlayerStatus))]
public class PlayerInventory : MonoBehaviour
{
    public ReactiveCollection<KnifeData_RunTime> runtimeKnives { get; private set; } = new ReactiveCollection<KnifeData_RunTime>();
    public ReactiveCollection<TreasureData> runtimeTreasure { get; private set; } = new();

    // アイテムとそのイベント購読を紐付けて管理する
    private readonly Dictionary<TreasureData, CompositeDisposable> _itemDisposables = new Dictionary<TreasureData, CompositeDisposable>();


    PlayerStatus status;

    private void Awake()
    {
        status = GetComponent<PlayerStatus>();
    }

    void Start()
    {

    }

    // ナイフを入手する処理
    public void AddKnife(KnifeData_RunTime x)
    {
        // その名前のナイフを既に持ってるか、持ってたら取得
        var known = runtimeKnives.FirstOrDefault(knife => knife._name == x._name);

        if (known == null)
        {
            // 渡されたナイフのデータを元に初期化したランタイム用ナイフデータ
            var y = new KnifeData_RunTime(x);

            // リストに加える
            runtimeKnives.Add(y);

            Debug.Log("Add");
        }
        else
        {
            // 持っていたら、そのナイフの重複度数をプラス
            known.count_Multiple++;
        }

        
    }

    public bool RemoveKnife(KnifeData_RunTime x)
    {
        if (!runtimeKnives.Contains(x)) return false;
        if (runtimeKnives.Count <= 1) return false;

        runtimeKnives.Remove(x);

        return true;
    }

    // 秘宝を入手する処理
    public void AddTreasure(TreasureData x)
    {
        //if (runtimeTreasure.Contains(x)) return; // 同じアイテムは追加しない

        // 渡されたデータのインスタンスを生成
        var y = Instantiate(x);

        // リストに加える
        runtimeTreasure.Add(y);

        // この秘宝に対するCompositeDisposableを新規作成
        var disposables = new CompositeDisposable();
        _itemDisposables.Add(y, disposables);

        // 秘宝の、入手時の処理を実行
        y.OnAdd(status, disposables);

        //Debug.Log($"{y._name} を取得した！");
    }

    public void RemoveTreasure(TreasureData x)
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

        //Debug.Log($"{x._name} を失った！");
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
