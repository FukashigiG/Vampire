using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_PlayerItemButton : MonoBehaviour
{
    [SerializeField] Image image_Knife;
    [SerializeField] Image image_Treasure;

    [SerializeField] GameObject body_MultipleCounter;
    [SerializeField] Text txt_MultipleCount;

    Button button;

    Base_PlayerItem holdData;

    [NonSerialized] public UnityEvent<Base_PlayerItem> onClicked = new UnityEvent<Base_PlayerItem> ();

    private void Awake()
    {
        button = GetComponent<Button>();

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onClicked?.Invoke(holdData));
    }

    public void SetData(Base_PlayerItem item)
    {
        holdData = item;

        image_Knife.gameObject.SetActive(false);
        image_Treasure.gameObject.SetActive(false);
        body_MultipleCounter.gameObject.SetActive(false);

        if(item is KnifeData knife)
        {
            image_Knife.gameObject.SetActive(true);

            image_Knife.sprite = knife.sprite;
        } 
        else if(item is TreasureData treasure)
        {
            image_Treasure.gameObject.SetActive(true);

            image_Treasure.sprite = treasure.sprite;
        }
        else if (item is KnifeData_RunTime runtime)
        {
            image_Knife.gameObject.SetActive(true);

            image_Knife.sprite = runtime.sprite;

            if(runtime.count_Multiple > 1)
            {
                body_MultipleCounter.SetActive(true);

                txt_MultipleCount.text = runtime.count_Multiple.ToString();
            }
        }
    }
}
