using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageTxtCtrler : MonoBehaviour
{
    [SerializeField] Text txt;

    [SerializeField] float life;

    public void Initialize(int dmg)
    {
        txt.text = dmg.ToString();
    }

    private void Update()
    {
        life -= Time.deltaTime;

        if (life < 0) Destroy(gameObject);
    }
}
