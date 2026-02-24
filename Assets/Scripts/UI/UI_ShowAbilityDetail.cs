using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UI_ShowAbilityDetail : SingletonMono<UI_ShowAbilityDetail>
{
    [SerializeField] TextMeshProUGUI txt_Name;
    [SerializeField] TextMeshProUGUI txt_Description;

    [SerializeField] GameObject body;

    public void Show(IDiscribing discribing)
    {
        body.SetActive(true);

        txt_Name.text = discribing._name;
        txt_Description.text = discribing.description;
    }

    public void Hide()
    {
        body.SetActive(false);
    }
}
