using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSceneManager : SingletonMono<TitleSceneManager>
{
    [SerializeField] StageDataHolder dataHolder;

    private void Awake()
    {
        Time.timeScale = 1.0f;
    }

    public void GoButtle()
    {
        dataHolder.SetData(CharaSelect_Director.Instance.cullentSelected);

        SceneLoader.Instance.Load("SampleScene");
    }
}
