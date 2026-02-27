using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UI_Manager : SingletonMono<UI_Manager>
{
    [SerializeField] InputActionAsset inputActions;

    [SerializeField] PausePanelCtrler _pausePanelCtrler;

    public void Initialize()
    {
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
