using UnityEngine;

public class PlayerDiedPanelDirector : SingletonMono<PlayerDiedPanelDirector>
{
    [SerializeField] GameObject bodyPanel;

    public void ShowPanel()
    {
        bodyPanel.SetActive(true);
    }
}
