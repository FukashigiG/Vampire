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

    [SerializeField] GameObject knifeButtonPrefab;

    [SerializeField] GameObject detailWindow;

    PlayerInventory playerInventory;

    // イベント購読をまとめて管理するためのDisposable
    private CompositeDisposable _disposables = new CompositeDisposable();

    private void Awake()
    {
        // 初期がアクティブでないオブジェクトへのアタッチを想定のため、AwakeやStartは上手く動作しない
    }

    // 初期化
    public void Initialize(InputAction action)
    {
        action.performed += TogglePanel; // 引数で渡されたアクションが実行されたらTogglePanelを呼ぶ

        playerInventory = PlayerController.Instance._status.inventory;

        // --- ナイフの購読設定 ---
        playerInventory.runtimeKnives
            .ObserveAdd()
            .Subscribe(e => OnKnifeAdded(e.Value))
            .AddTo(_disposables);

        playerInventory.runtimeKnives
            .ObserveRemove()
            .Subscribe(e => OnKnifeRemoved(e.Value))
            .AddTo(_disposables);

        // --- トレジャーの購読設定 ---
        playerInventory.runtimeTreasure
            .ObserveAdd()
            .Subscribe(e => OnTreasureAdded(e.Value))
            .AddTo(_disposables);

        playerInventory.runtimeTreasure
            .ObserveRemove()
            .Subscribe(e => OnTreasureRemoved(e.Value))
            .AddTo(_disposables);

        Debug.Log("set toggle");

        foreach (var knives in playerInventory.runtimeKnives) OnKnifeAdded(knives);
        foreach (var treasures in playerInventory.runtimeTreasure) OnTreasureAdded(treasures);
    }

    void OnKnifeAdded(KnifeData knifeData)
    {

    }

    void OnKnifeRemoved(KnifeData knifeData)
    {

    }


    void OnTreasureAdded(Base_TreasureData treasureData)
    {

    }

    void OnTreasureRemoved(Base_TreasureData treasureData)
    {

    }

    // パネル表示
    void TogglePanel(InputAction.CallbackContext context)
    {
        this.gameObject.SetActive(true);
    }

    /*
    // 表示されたとき
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

    // パネル非表示
    public void CloseThis()
    {
        this.gameObject.SetActive(false);
    }

    // このオブジェクトが破棄されるときに購読をすべて解除
    private void OnDestroy()
    {
        _disposables.Dispose();
    }

    // 非表示になったとき
    private void OnDisable()
    {
        
    }
}
