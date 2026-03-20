using TMPro;
using UnityEngine;

public class Window_ShowDetail : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txt_Name;
    [SerializeField] TextMeshProUGUI txt_Description;

    public void OpenWindow(IDiscribing discribing)
    {
        this.gameObject.SetActive(true);

        txt_Name.text = discribing._name;
        txt_Description.text = discribing.description;
    }
}
