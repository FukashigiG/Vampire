using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Detail_KnifeAbility : SingletonMono<UI_Detail_KnifeAbility>
{
    [SerializeField] Text txt_Name;
    [SerializeField] Text txt_Discription;

    [SerializeField] GameObject body;

    public void Show(KnifeAbility ability)
    {
        body.SetActive(true);

        txt_Name.text = ability.abilityLogic.effectName;
        txt_Discription.text = ability.abilityLogic.description;
    }

    public void Hide()
    {
        body.SetActive(false);
    }
}
