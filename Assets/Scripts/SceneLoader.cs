using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class SceneLoader : SM_DontDestOnLand<SceneLoader>
{
    protected override bool dontDestroyOnLoad { get { return true; } }

    [SerializeField] GameObject fader_Prefab;

    public async void Load(string Scene)
    {
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, Vector2.zero);

        // フェーダーを非破壊キャンバスに生成
        GameObject x = Instantiate(fader_Prefab, screenPoint, Quaternion.identity, DontDestroiedCanvas.Instance.transform);

        var y = x.GetComponent<UI_FadeAnimator>();

        await y.FadeOut();

        await SceneManager.LoadSceneAsync(Scene);

        await y.FadeIn();

        Destroy(x);
    }
}
