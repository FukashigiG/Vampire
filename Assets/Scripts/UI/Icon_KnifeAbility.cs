using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Icon_KnifeAbility : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Text txt_Name;

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
    }

    public void OnPointerEnter(PointerEventData data)
    {
        // 詳細ウインドウの表示

        UI_Detail_KnifeAbility.Instance.Show(knifeAbility);

        //detailWindow.SetActive(true);
    }

    // カーソルが外れたとき
    public void OnPointerExit(PointerEventData data)
    {
        UI_Detail_KnifeAbility.Instance.Hide();

        //detailWindow.SetActive(false);
    }
}
