using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Linq;

public class PausePanelCtrler : MonoBehaviour
{
    [SerializeField] Transform knifeArea;

    [SerializeField] GameObject knifeButtonPrefab;

    [SerializeField] GameObject detailWindow;

    private void Awake()
    {
        // 初期がアクティブでないオブジェクトへのアタッチを想定のため、AwakeやStartは上手く動作しない
    }

    // 初期化
    public void Initialize(InputAction action)
    {
        action.performed += TogglePanel; // 引数で渡されたアクションが実行されたらTogglePanelを呼ぶ

        Debug.Log("set toggle");
    }

    // パネル表示
    void TogglePanel(InputAction.CallbackContext context)
    {
        this.gameObject.SetActive(true);
    }

    // 表示されたとき
    private void OnEnable()
    {
        List<KnifeData> knives = PlayerController.Instance._status.inventory.runtimeKnives.ToList();

        foreach(var knifeData in knives)
        {
            var x = Instantiate(knifeButtonPrefab, knifeArea);

            x.GetComponent<Button_AddKnifeCtrler>().Initialize(knifeData);
        }
    }

    // パネル非表示
    public void CloseThis()
    {
        // 全削除
        foreach(Transform button in knifeArea)
        {
            Destroy(button.gameObject);
        }

        this.gameObject.SetActive(false);
    }

    // 非表示になったとき
    private void OnDisable()
    {
        
    }
}
