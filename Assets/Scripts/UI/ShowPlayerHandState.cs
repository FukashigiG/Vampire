using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowPlayerHandState : MonoBehaviour
{
    [SerializeField] GameObject KnifeImagePrefeb;
    [SerializeField] float width;

    public void AddedKnifeInHand(Sprite sprite, int index)
    {
        GameObject x = Instantiate(KnifeImagePrefeb, Vector2.zero, Quaternion.identity, this.transform);

        x.transform.GetChild(0).GetComponent<Image>().sprite = sprite;

        var rect = x.GetComponent<RectTransform>();

        rect.SetSiblingIndex(index);

        rect.anchoredPosition = Vector2.up * width * -index;
    }

    public void RemoveKnifeInHand(int index)
    {
        Destroy(this.transform.GetChild(index).gameObject);
    }

    public void ResetAll()
    {
        foreach (Transform t in this.transform) Destroy(t.gameObject);
    }
}
