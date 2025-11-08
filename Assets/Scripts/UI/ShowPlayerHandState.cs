using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPlayerHandState : MonoBehaviour
{
    [SerializeField] GameObject KnifeImagePrefeb;

    public void ShowReloadResult(List<Sprite> sprites)
    {
        foreach (Transform x in this.transform)
        {
            Destroy(x.gameObject);
        }

        for (int i = 0; i < sprites.Count; i++)
        {
            Instantiate(KnifeImagePrefeb, Vector2.zero, Quaternion.identity, this.transform);
        }
    }
}
