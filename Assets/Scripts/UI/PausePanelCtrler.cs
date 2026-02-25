using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Linq;
using UniRx;
using System.Collections.Specialized;

public class PausePanelCtrler : MonoBehaviour
{
    [SerializeField] GameObject body;

    // 一覧となるオブジェクトのTransform
    // Instantiate時に親の引数として渡す
    [SerializeField] Transform knifeArea;
    [SerializeField] Transform treasureArea;

    // 生成されるもの
    [SerializeField] GameObject prefab_ItemButton;

    PlayerInventory playerInventory;

    // イベント購読をまとめて管理するためのDisposable
    private CompositeDisposable _disposables = new CompositeDisposable();

    // 所持ナイフ/秘宝欄に生成されたオブジェクトを管理するためのもの
    Dictionary<KnifeData_RunTime, GameObject> knifeImageDictionaty = new();
    Dictionary<TreasureData, GameObject> treasureImageDictionaty = new();

    [SerializeField] Button btn_ClosePanel;
    [SerializeField] Button btn_Setting;
    [SerializeField] Button btn_Retire;

    InputAction _inputaction;

    private void Awake()
    {

    }

    // 初期化
    public void Initialize(InputAction action)
    {
        _inputaction = action;

        _inputaction.performed += TogglePanel; // 引数で渡されたアクションが実行されたらTogglePanelを呼ぶ

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

        btn_ClosePanel.onClick.AddListener(() =>
        {
            CloseThis();
        });

        btn_Retire.onClick.AddListener(() =>
        {
            CloseThis();

            GameAdmin.Instance.GameSet(false);
        });

        btn_Setting.onClick.AddListener(() =>
        {
            UI_Setting.Instance.OpenPanel();
        });
    }

    // ナイフが追加された際
    void OnKnifeAdded(KnifeData_RunTime knifeData)
    {
        // ナイフ一覧に新たに生成
        var itemButton = Instantiate(prefab_ItemButton, knifeArea).GetComponent<UI_PlayerItemButton>();
        itemButton.SetData(knifeData);

        // 辞書に登録
        knifeImageDictionaty[knifeData] = itemButton.gameObject;

        itemButton.onClicked.AddListener((x) =>
        {
            // ヒエラルキー参照でボタンが何番目か取得する
            int index = itemButton.transform.GetSiblingIndex();

            UI_ShowPlayerItemInfo.Instance.ShowPanel(knifeImageDictionaty.Keys.ToList<Base_PlayerItem>(), index);
        });
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
    void OnTreasureAdded(TreasureData treasureData)
    {
        var itemBtn = Instantiate(prefab_ItemButton, treasureArea).GetComponent<UI_PlayerItemButton>();
        itemBtn.SetData(treasureData);

        treasureImageDictionaty[treasureData] = itemBtn.gameObject;

        itemBtn.onClicked.AddListener((x) =>
        {
            // ヒエラルキー参照でボタンが何番目か取得する
            int index = itemBtn.transform.GetSiblingIndex();

            UI_ShowPlayerItemInfo.Instance.ShowPanel(treasureImageDictionaty.Keys.ToList<Base_PlayerItem>(), index);
        });
    }

    // 秘宝が削除された際
    void OnTreasureRemoved(TreasureData treasureData)
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
        if(body.activeSelf == true) return;

        if(GameAdmin.Instance.isPausing) return;

        body.SetActive(true);

        GameAdmin.Instance.PauseGame();
    }

    // パネル非表示
    public void CloseThis()
    {
        if(body.activeSelf == false) return;

        body.SetActive(false);



        GameAdmin.Instance.ResumeGame();
    }

    // このオブジェクトが破棄されるときに、購読をすべて解除
    private void OnDestroy()
    {
        _disposables.Dispose();

        _inputaction.performed -= TogglePanel;
    }

    // 非表示になったとき
    private void OnDisable()
    {
        
    }
}
