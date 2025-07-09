using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectSceneManager : SingletonMono<SelectSceneManager>
{
    public void GoMainSampleScene()
    {
        SceneLoader.Instance.Load("SampleScene");
    }
}
