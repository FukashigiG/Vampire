using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Viewer_StageSelect : SingletonMono<Viewer_StageSelect>
{
    [SerializeField] GameObject body_Panel;

    [SerializeField] Button[] buttons_Option;
    [SerializeField] Button button_Decide;

    [SerializeField] Text txt_StageName;

    [SerializeField] List<StageData> list_Stage_Wave;

    StageData currentSelected = null;

    private void Awake()
    {
        button_Decide.onClick.AddListener(Decision);
    }

    public void ShowEvent()
    {
        body_Panel.SetActive(true);

        foreach (var button in buttons_Option)
        {
            button.gameObject.SetActive(true);

            button.onClick.RemoveAllListeners();

            var RandomStage = list_Stage_Wave[0];

            button.onClick.AddListener(() =>
            {
                ShowDiscription(RandomStage);
            });
        }

        currentSelected = null ;
    }

    void ShowDiscription(StageData data)
    {
        currentSelected = data ;

        txt_StageName.text = data.stageName;
    }

    void Decision()
    {
        if (currentSelected == null) return;

        GameAdmin.Instance.UpdateWave(currentSelected);

        ClosePanel();
    }

    void ClosePanel()
    {
        body_Panel.SetActive(false);
    }
}
