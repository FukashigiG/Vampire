using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSceneManager : SingletonMono<TitleSceneManager>
{
    public void GoSelectScene()
    {
        SceneLoader.Instance.Load("SelectScene");
    }
}
