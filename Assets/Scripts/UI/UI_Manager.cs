using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UI_Manager : SingletonMono<UI_Manager>
{
    [SerializeField] InputActionAsset inputActions;

    [SerializeField] PausePanelCtrler _pausePanelCtrler;

    private void Awake()
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
            togglePanelAction.Enable(); // この一文があって初めて、アクションが呼ばれるようになる

            _pausePanelCtrler.Initialize(togglePanelAction);
        }
        else
        {
            Debug.Log("Actionが見つかりません");
        }
        
    }
}
