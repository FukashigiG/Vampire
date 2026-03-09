using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UI_Manager : SingletonMono<UI_Manager>
{
    [SerializeField] InputActionAsset inputActions;

    [SerializeField] PausePanelCtrler _pausePanelCtrler;

    [SerializeField] GameObject[] UI_ForPC;
    [SerializeField] GameObject[] UI_ForSumaho;

    public void Initialize(bool isForPC)
    {
        foreach (GameObject go in UI_ForPC)
        {
            if (go != null) go.SetActive(isForPC);
        }

        foreach(GameObject go in UI_ForSumaho)
        {
            if(go != null) go.SetActive(! isForPC);
        }

        InputActionMap uiMap = inputActions.FindActionMap("UICtrl");
        InputAction togglePanelAction = null;

        if (uiMap != null)
        {
            togglePanelAction = uiMap.FindAction("TogglePanel");
        }
        else
        {
            Debug.Log("ActionMapが見つかりません");
        }

        if(togglePanelAction != null)
        {
            // すでに有効化されていないかチェック
            if (!togglePanelAction.enabled)
            {
                togglePanelAction.Enable(); // この一文があって初めて、アクションが呼ばれるようになる
            }

            _pausePanelCtrler.Initialize(togglePanelAction, PlayerController.Instance._status);
        }
        else
        {
            Debug.Log("Actionが見つかりません");
        }
        
    }
}
