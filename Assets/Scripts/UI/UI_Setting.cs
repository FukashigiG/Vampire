using UnityEngine;
using UnityEngine.UI;

public class UI_Setting : SingletonMono<UI_Setting>
{
    [SerializeField] GameObject body;

    [SerializeField] Button btn_Close;

    public void Initialize()
    {
        btn_Close.onClick.RemoveAllListeners();

        btn_Close.onClick.AddListener(ClosePanel);
    }

    public void OpenPanel()
    {
        body.SetActive(true);
    }

    void ClosePanel()
    {
        body?.SetActive(false);
    }
}
