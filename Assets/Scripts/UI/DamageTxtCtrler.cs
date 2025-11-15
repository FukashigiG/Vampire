using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Random = UnityEngine.Random;

public class DamageTxtCtrler : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txt;

    [SerializeField] float life;

    public void Initialize(int dmg)
    {
        GetComponent<RectTransform>().anchoredPosition += new Vector2(Random.Range(-10f, 10f), Random.Range(-10f, 10f));

        txt.text = dmg.ToString();

        transform.localScale = new Vector3(2.4f, 2.4f, 1f);

        transform.DOScale(Vector3.one, 0.4f);
    }

    private void Update()
    {
        life -= Time.deltaTime;

        if (life < 0) Destroy(gameObject);
    }
}
