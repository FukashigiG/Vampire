using UnityEngine;
using UnityEngine.UI;

public class UI_Setting : SingletonMono<UI_Setting>
{
    [SerializeField] GameObject body;

    [SerializeField] Button btn_Close;

    public void Initialize(Button btn_OpenThis)
    {
        btn_OpenThis.onClick.RemoveAllListeners();
        btn_Close.onClick.RemoveAllListeners();

        btn_OpenThis.onClick.AddListener(OpenPanel);
        btn_Close.onClick.AddListener(ClosePanel);
    }

    void OpenPanel()
    {
        body.SetActive(true);
    }

    void ClosePanel()
    {
        body?.SetActive(false);
    }
}
