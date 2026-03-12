using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class DamageTxtFactory : SingletonMono<DamageTxtFactory>
{
    [SerializeField] GameObject txtPrefab;

    [SerializeField] Camera camera_UI;

    [SerializeField] RectTransform thisRect;

    public void InstantiateDmgTxt(Vector2 worldPoint, int damageValue, Color color)
    {
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(worldPoint);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            thisRect,
            screenPoint,
            camera_UI,
            out var localPoint
        );

        var x = Instantiate(txtPrefab, this.transform);
        x.GetComponent<RectTransform>().localPosition = localPoint;

        x.GetComponent<DamageTxtCtrler>().Initialize(damageValue, color);
    }
}
