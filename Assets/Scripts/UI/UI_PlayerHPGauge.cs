using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerHPGauge : MonoBehaviour
{
    [SerializeField] Image gauge_PlayerHP;
    [SerializeField] Text txt_PlayerHP;

    void Start()
    {
        
    }

    public void SetGauge(int hp, int maxHp)
    {
        gauge_PlayerHP.fillAmount = ((float)hp / (float)maxHp);

        txt_PlayerHP.text = $"HP : {hp} / {maxHp}";
    }
}
