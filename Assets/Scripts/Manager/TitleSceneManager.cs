using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleSceneManager : SingletonMono<TitleSceneManager>
{
    [SerializeField] StageDataHolder dataHolder;

    [SerializeField] Button btn_Setting;
    [SerializeField] Button btn_SelectChara;
    [SerializeField] Button btn_Dictionary;

    protected override void Awake()
    {
        base.Awake();

        Time.timeScale = 1.0f;

        UI_Setting.Instance.Initialize();
        CharaSelect_Director.Instance.Initialize(btn_SelectChara);
        EnemyDictionary_Director.Instance.Initialize(btn_Dictionary);

        btn_Setting.onClick.RemoveAllListeners();
        btn_Setting.onClick.AddListener(() => UI_Setting.Instance.OpenPanel());
    }

    public void GoButtle()
    {
        dataHolder.SetData(CharaSelect_Director.Instance.cullentSelected);

        SceneLoader.Instance.Load("SampleScene");
    }
}
