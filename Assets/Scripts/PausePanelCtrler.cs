using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePanelCtrler : MonoBehaviour
{
    [SerializeField] Transform knifeArea;

    [SerializeField] Button b;

    [SerializeField] GameObject knifeButtonPrefab;



    private void OnEnable()
    {
        List<KnifeData> knives = PlayerController.Instance._attack.availableKnifes;

        foreach(var knifeData in knives)
        {
            var x = Instantiate(knifeButtonPrefab, knifeArea);

            x.GetComponent<Button_AddKnifeCtrler>().Initialize(knifeData);
        }
    }

    public void CloseThis()
    {
        foreach(Transform button in knifeArea)
        {
            Destroy(button.gameObject);
        }

        this.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        
    }
}
