using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class TitleSceneManager : SingletonMono<TitleSceneManager>
{
    [SerializeField] StageDataHolder dataHolder;

    [SerializeField] Button btn_Setting;
    [SerializeField] Button btn_SelectChara;
    [SerializeField] Button btn_Dictionary;
    [SerializeField] Button btn_Tips;

    [SerializeField] Toggle toggle_EndLessMode;
    [SerializeField] Toggle toggle_PlayForSmaho;


    bool endLessMode = false;
    bool isPlayForSmaho = false;

    protected override void Awake()
    {
        base.Awake();

        Time.timeScale = 1.0f;

        UI_Setting.Instance.Initialize();
        CharaSelect_Director.Instance.Initialize(btn_SelectChara);
        TipsDictionary.Instance.Initialize(btn_Tips);

        List<EnemyData> enemies = Resources.LoadAll<EnemyData>("GameDatas/Enemy").ToList();
        EnemyDictionary_Director.Instance.Initialize(btn_Dictionary, enemies);

        btn_Setting.onClick.RemoveAllListeners();
        btn_Setting.onClick.AddListener(() => UI_Setting.Instance.OpenPanel());

        toggle_EndLessMode.onValueChanged.RemoveAllListeners();
        toggle_EndLessMode.onValueChanged.AddListener((value) =>
        {
            endLessMode = value;
        });

        toggle_PlayForSmaho.onValueChanged.RemoveAllListeners();
        toggle_PlayForSmaho.onValueChanged.AddListener((value) =>
        {
            isPlayForSmaho = value;

            Screen.fullScreen = ! Screen.fullScreen;
        });

        toggle_EndLessMode.isOn = dataHolder.isEndless;
        toggle_PlayForSmaho.isOn = !dataHolder.isPlayOnPC;
    }

    public void GoButtle()
    {
        dataHolder.SetData(CharaSelect_Director.Instance.cullentSelected, endLessMode, _isPlayOnPC: !isPlayForSmaho);

        SceneLoader.Instance.Load("SampleScene");
    }
}
