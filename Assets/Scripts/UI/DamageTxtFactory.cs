using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class DamageTxtFactory : SingletonMono<DamageTxtFactory>
{
    [SerializeField] GameObject txtPrefab;

    [SerializeField] Camera camera_UI;

    [SerializeField] RectTransform thisRect;

    List<DamageTxtCtrler> txts = new();

    public void DisplayDmgTxt(Vector2 worldPoint, int damageValue, Color color)
    {
        DamageTxtCtrler txtCtrler = txts.Where(t => t.gameObject.activeSelf == false).FirstOrDefault();

        if (txtCtrler == null) txtCtrler = InstantiateDmgTxt();

        Vector2 screenPoint = Camera.main.WorldToScreenPoint(worldPoint);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            thisRect,
            screenPoint,
            camera_UI,
            out var localPoint
        );

        txtCtrler.Initialize(damageValue, color, localPoint);
    }

    public DamageTxtCtrler InstantiateDmgTxt()
    {
        var x = Instantiate(txtPrefab, this.transform);

        var ctrl = x.GetComponent<DamageTxtCtrler>();

        txts.Add(ctrl);

        return ctrl;
    }
}
