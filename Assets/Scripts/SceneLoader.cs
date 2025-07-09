using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class SceneLoader : SM_DontDestOnLand<SceneLoader>
{
    protected override bool dontDestroyOnLoad { get { return true; } }

    public async void Load(string Scene)
    {
        await SceneManager.LoadSceneAsync(Scene);
    }
}
