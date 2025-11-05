using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Detail_HSpE : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject detailWindow;

    [SerializeField] Text txt_Name;
    [SerializeField] Text txt_Name_DetailWindow;
    [SerializeField] Text txt_Description;

    Base_KnifeAbility knifeAbility;

    public void Initialize(Base_KnifeAbility _knifeAbility)
    {
        knifeAbility = _knifeAbility;

        txt_Name.text = knifeAbility.effectName;
        txt_Name_DetailWindow.text = knifeAbility.effectName;
        txt_Description.text = knifeAbility.description;

        detailWindow.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData data)
    {
        // 詳細ウインドウの表示
        detailWindow.SetActive(true);
    }

    // カーソルが外れたとき
    public void OnPointerExit(PointerEventData data)
    {
        detailWindow.SetActive(false);
    }
}
