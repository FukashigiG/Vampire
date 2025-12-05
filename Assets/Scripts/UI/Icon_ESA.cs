using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Icon_ESA : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image iconImage;

    Base_EnemyStatusAbilityData esad;

    public void Initialize(Base_EnemyStatusAbilityData _esad)
    {
        if(_esad == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        esad = _esad;

        iconImage.sprite = esad.icon;
    }

    public void OnPointerEnter(PointerEventData data)
    {
        // 詳細ウインドウの表示

        UI_ShowAbilityDetail.Instance.Show(esad);

        //detailWindow.SetActive(true);
    }

    // カーソルが外れたとき
    public void OnPointerExit(PointerEventData data)
    {
        UI_ShowAbilityDetail.Instance.Hide();

        //detailWindow.SetActive(false);
    }
}
