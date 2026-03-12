using UnityEngine;
using UnityEngine.UI;

public class Button_GetSome : MonoBehaviour
{
    [field:SerializeField] public Button btn {  get; private set; }

    [SerializeField] Image _image;

    [SerializeField] ParticleSystem fx_Rare2;
    [SerializeField] ParticleSystem fx_Rare3;

    public void SetUp(Base_PlayerItem item)
    {
        _image.sprite = item.sprite;

        fx_Rare2.gameObject.SetActive(false);
        fx_Rare3.gameObject.SetActive(false);

        switch(item.rank)
        {
            case 2:
                fx_Rare2.gameObject.SetActive(true);
                //fx_Rare2.Play();
                break;

            case 3:
                fx_Rare3.gameObject.SetActive(true);
                //fx_Rare3.Play();
                break;

            default:
                break;
        }
    }
}
