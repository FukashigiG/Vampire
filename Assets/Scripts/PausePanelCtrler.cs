using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Linq;
using UniRx;

public class PausePanelCtrler : MonoBehaviour
{
    [SerializeField] GameObject body;

    // 一覧となるオブジェクトのTransform
    // Instantiate時に親の引数として渡す
    [SerializeField] Transform knifeArea;
    [SerializeField] Transform treasureArea;

    // 生成される
    [SerializeField] GameObject knifeImagePrefab;
    [SerializeField] GameObject treasureImagePrefab;

    PlayerInventory playerInventory;

    // イベント購読をまとめて管理するためのDisposable
    private CompositeDisposable _disposables = new CompositeDisposable();

    // 所持ナイフ/秘宝欄に生成されたオブジェクトを管理するためのもの
    Dictionary<KnifeData_RunTime, GameObject> knifeImageDictionaty = new();
    Dictionary<Base_TreasureData, GameObject> treasureImageDictionaty = new();

    private void Awake()
    {
        // 初期がアクティブでないオブジェクトへのアタッチを想定のため、AwakeやStartは上手く動作しない
    }

    // 初期化
    public void Initialize(InputAction action)
    {
        action.performed += TogglePanel; // 引数で渡されたアクションが実行されたらTogglePanelを呼ぶ

        playerInventory = PlayerController.Instance._status.inventory; // PlayerInventoryをプレイヤーから取得

        // --- ナイフの購読設定 ---
        playerInventory.runtimeKnives
            .ObserveAdd()
            .Subscribe(e => OnKnifeAdded(e.Value))
            .AddTo(_disposables);

        playerInventory.runtimeKnives
            .ObserveRemove()
            .Subscribe(e => OnKnifeRemoved(e.Value))
            .AddTo(_disposables);

        // --- 秘宝の購読設定 ---
        playerInventory.runtimeTreasure
            .ObserveAdd()
            .Subscribe(e => OnTreasureAdded(e.Value))
            .AddTo(_disposables);

        playerInventory.runtimeTreasure
            .ObserveRemove()
            .Subscribe(e => OnTreasureRemoved(e.Value))
            .AddTo(_disposables);

        // 最初に、既に登録されている物について、追加処理をする
        foreach (var knives in playerInventory.runtimeKnives) OnKnifeAdded(knives);
        foreach (var treasures in playerInventory.runtimeTreasure) OnTreasureAdded(treasures);
    }

    // ナイフが追加された際
    void OnKnifeAdded(KnifeData_RunTime knifeData)
    {
        // ナイフ一覧に新たに生成
        var imageObj = Instantiate(knifeImagePrefab, knifeArea);
        imageObj.GetComponent<Image>().sprite = knifeData.sprite;

        // 辞書に登録
        knifeImageDictionaty[knifeData] = imageObj;
    }

    // ナイフが削除された際
    void OnKnifeRemoved(KnifeData_RunTime knifeData)
    {
        // 辞書に登録されているものであれば
        if (knifeImageDictionaty.TryGetValue(knifeData, out var obj))
        {
            // 要素を削除
            Destroy(obj);
            knifeImageDictionaty.Remove(knifeData);
        }
    }

    // 秘宝が追加された際
    void OnTreasureAdded(Base_TreasureData treasureData)
    {
        var imageObj = Instantiate(treasureImagePrefab, treasureArea);
        imageObj.GetComponent<Image>().sprite = treasureData.icon;

        treasureImageDictionaty[treasureData] = imageObj;
    }

    // 秘宝が削除された際
    void OnTreasureRemoved(Base_TreasureData treasureData)
    {
        if (treasureImageDictionaty.TryGetValue(treasureData, out var obj))
        {
            Destroy(obj);
            treasureImageDictionaty.Remove(treasureData);
        }
    }

    // パネル表示
    void TogglePanel(InputAction.CallbackContext context)
    {
        body.SetActive(true);

        GameAdmin.Instance.PauseGame();
    }

    // パネル非表示
    public void CloseThis()
    {
        body.SetActive(false);

        GameAdmin.Instance.ResumeGame();
    }

    // このオブジェクトが破棄されるときに、購読をすべて解除
    private void OnDestroy()
    {
        _disposables.Dispose();
    }

    // 非表示になったとき
    private void OnDisable()
    {
        
    }
}
