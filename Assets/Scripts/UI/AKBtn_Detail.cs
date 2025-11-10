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
    [SerializeField] GameObject area_HSpE;
    [SerializeField] GameObject HSpE_TxtObj;
    [SerializeField] Text t_Description;

    KnifeData_RunTime knifeData;

    private void OnEnable()
    {
        
    }

    // 初期化処理
    public void Initialize(KnifeData_RunTime data)
    {
        knifeData = data;

        t_Name.text = knifeData._name;
        t_Rare.text = "Rarity : " + knifeData.rarity;
        t_Attribute.text = "Attribute : " + knifeData.element;
        t_Power.text = "Attack : " + knifeData.power;

        t_Description.text = "";

        foreach(Transform child in area_HSpE.transform)
        {
            Destroy(child.gameObject);
        }

        // このナイフが持つ特殊能力を記述する処理
        foreach (var effect in knifeData.abilities)
        {
            if(effect != null)
            {
                GameObject a = Instantiate(HSpE_TxtObj, area_HSpE.transform);

                var d = a.GetComponent<Detail_HSpE>();

                d.Initialize(effect);
            }
        }

        t_Description.text += knifeData.description;

    }
}
