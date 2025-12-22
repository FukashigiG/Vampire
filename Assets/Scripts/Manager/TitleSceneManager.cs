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

    private void Awake()
    {
        Time.timeScale = 1.0f;

        UI_Setting.Instance.Initialize(btn_Setting);
        CharaSelect_Director.Instance.Initialize(btn_SelectChara);
        EnemyDictionary_Director.Instance.Initialize(btn_Dictionary);
    }

    public void GoButtle()
    {
        dataHolder.SetData(CharaSelect_Director.Instance.cullentSelected);

        SceneLoader.Instance.Load("SampleScene");
    }
}
