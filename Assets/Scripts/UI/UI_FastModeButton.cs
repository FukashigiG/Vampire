using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UI_FastModeButton : SingletonMono<UI_FastModeButton>
{
    [SerializeField] Image img_On;

    InputAction input_TimeScaleMode;

    bool onBoost = false;

    public void Initialize(InputAction input)
    {
        input_TimeScaleMode = input;

        input_TimeScaleMode.performed += Set;
    }

    void Set(InputAction.CallbackContext context)
    {
        if(GameAdmin.Instance.cullentTimeScale != 1f) return;

        onBoost = ! onBoost;

        if (onBoost)
        {
            GameAdmin.Instance.SetTimeScaleMultiplier(2);

            img_On.enabled = true;
        }
        else
        {
            GameAdmin.Instance.SetTimeScaleMultiplier(1);

            img_On.enabled = false;
        }
    }

    private void OnDestroy()
    {
        input_TimeScaleMode.performed -= Set;
    }
}
