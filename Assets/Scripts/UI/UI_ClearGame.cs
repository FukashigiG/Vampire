using UnityEngine;

public class UI_ClearGame : SingletonMono<UI_ClearGame>
{
    [SerializeField] GameObject body;

    public void ShowPanel()
    {
        body.SetActive(true);
    }
}
