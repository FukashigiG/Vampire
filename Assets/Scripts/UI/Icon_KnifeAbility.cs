using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Icon_KnifeAbility : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject detailWindow;

    [SerializeField] Text txt_Name;
    [SerializeField] Text txt_Name_DetailWindow;
    [SerializeField] Text txt_Description;

    KnifeAbility knifeAbility;

    public void Initialize(KnifeAbility _knifeAbility)
    {
        if(_knifeAbility == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        knifeAbility = _knifeAbility;

        txt_Name.text = knifeAbility.abilityLogic.effectName;
        txt_Name_DetailWindow.text = knifeAbility.abilityLogic.effectName;
        txt_Description.text = knifeAbility.abilityLogic.description;

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
