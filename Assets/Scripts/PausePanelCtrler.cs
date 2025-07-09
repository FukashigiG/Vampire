using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PausePanelCtrler : MonoBehaviour
{
    [SerializeField] Transform knifeArea;

    [SerializeField] GameObject knifeButtonPrefab;



    private void Awake()
    {
        // 初期がアクティブでないオブジェクトへのアタッチを想定のため、AwakeやStartは上手く動作しない
    }

    // 引数で渡されたアクションが実行されたらTogglePanelを呼ぶ
    public void Initialize(InputAction action)
    {
        action.performed += TogglePanel;

        Debug.Log("set toggle");
    }

    void TogglePanel(InputAction.CallbackContext context)
    {
        this.gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        List<KnifeData> knives = PlayerController.Instance._attack.availableKnifes;

        foreach(var knifeData in knives)
        {
            var x = Instantiate(knifeButtonPrefab, knifeArea);

            x.GetComponent<Button_AddKnifeCtrler>().Initialize(knifeData);
        }
    }

    public void CloseThis()
    {
        foreach(Transform button in knifeArea)
        {
            Destroy(button.gameObject);
        }

        this.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        
    }
}
