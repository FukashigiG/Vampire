using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowPlayerHandState : MonoBehaviour
{
    [SerializeField] GameObject KnifeImagePrefeb;
    [SerializeField] float width;

    public void ShowReloadResult(List<Sprite> sprites)
    {
        foreach (Transform x in this.transform)
        {
            Destroy(x.gameObject);
        }

        for (int i = 0; i < sprites.Count; i++)
        {
            GameObject x = Instantiate(KnifeImagePrefeb, Vector2.zero, Quaternion.identity, this.transform);

            x.transform.GetChild(0).GetComponent<Image>().sprite = sprites[i];

            x.GetComponent<RectTransform>().anchoredPosition = Vector2.up * width * -i;
        }
    }

    public void Thrown(int index)
    {
        Destroy(this.transform.GetChild(0).gameObject);
    }
}
