using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AKBtn_Detail : MonoBehaviour
{
    [SerializeField] Text t_Name;
    [SerializeField] Text t_Rare;
    [SerializeField] Text t_Attribute;
    [SerializeField] Text t_Power;
    [SerializeField] Text t_Description;

    KnifeData knifeData;

    private void OnEnable()
    {
        
    }

    public void Initialize(KnifeData data)
    {
        knifeData = data;

        t_Name.text = knifeData._name;
        t_Rare.text = "Rarity : " + knifeData.rarity;
        t_Attribute.text = "Attribute : " + knifeData.element;
        t_Power.text = "Attack : " + knifeData.power;

        t_Description.text = ""; // èâä˙âª

        foreach (var effect in knifeData.specialEffects)
        {
            if(effect != null)
            {
                t_Description.text += "<color=#CCD900>" + effect.effectName + "</color>\n";
            }
        }

        t_Description.text += knifeData.description;

    }
}
