using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowPlayerHandState : MonoBehaviour
{
    [SerializeField] GameObject KnifeImagePrefeb;
    [SerializeField] float width;

    List<UI_KnifeImg_ShowHand> knifeImgs = new List<UI_KnifeImg_ShowHand> ();

    public void AddedKnifeInHand(Sprite sprite, int index)
    {
        GameObject x = Instantiate(KnifeImagePrefeb, Vector2.zero, Quaternion.identity, this.transform);

        var copmonent = x.transform.GetComponent<UI_KnifeImg_ShowHand>();

        knifeImgs.Add(copmonent);

        copmonent.Initialize(sprite);

        var rect = x.GetComponent<RectTransform>();

        rect.SetSiblingIndex(index);

        rect.anchoredPosition = Vector2.up * (width * -index - 50f);
    }

    public void RemoveKnifeInHand(int index)
    {
        knifeImgs[index].DisappearAnim();

        knifeImgs.RemoveAt(index);
    }

    public void ResetAll()
    {
        foreach (var img in knifeImgs)
        {
            img.DisappearAnim();
        }

        knifeImgs.Clear();
    }
}
