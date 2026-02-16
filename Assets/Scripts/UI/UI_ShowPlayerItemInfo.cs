using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI_ShowPlayerItemInfo : SingletonMono<UI_ShowPlayerItemInfo>
{
    // 武器・秘宝獲得発動時、その内容を表示するUI用スクリプト

    [field: SerializeField] public GameObject body_Panel { get; private set; }

    [SerializeField] Button button_ShowNext;
    [SerializeField] Button button_ShowBack;
    [SerializeField] Button button_Close;

    [SerializeField] Image icon;

    [SerializeField] Text txt_Type;
    [SerializeField] Text txt_ItemName;
    [SerializeField] Text txt_Rareity;
    [SerializeField] Text txt_Element;

    [SerializeField] GameObject descriptionArea_Knife;
    [SerializeField] Text txt_BaseATK_Knife;
    [SerializeField] Text txt_ElementATK_Knife;
    [SerializeField] Text txt_Multiple_Knife;
    [SerializeField] Icon_KnifeAbility[] KA_Icons;

    [SerializeField] GameObject descriptionArea_Treasure;
    [SerializeField] Text description_Treasure;

    enum ItemType { knife, treasure }

    List<Base_PlayerItem> items;

    int cullentIndex;

    protected override void Awake()
    {
        base.Awake();


        button_ShowNext.onClick.RemoveAllListeners();
        button_ShowNext.onClick.AddListener(GoNext);

        button_ShowBack.onClick.RemoveAllListeners();
        button_ShowBack.onClick.AddListener(GoBack);

        button_Close.onClick.RemoveAllListeners();
        button_Close.onClick.AddListener(ClosePanel);
    }

    public void ShowPanel(List<Base_PlayerItem> _items, int index)
    {
        items = _items;

        cullentIndex = index;

        body_Panel.SetActive(true);

        ShowDiscription(items[index]);
    }

    void GoNext()
    {
        int x = cullentIndex + 1;

        if (x >= items.Count) return;

        cullentIndex = x;

        ShowDiscription(items[cullentIndex]);
    }

    void GoBack()
    {
        int x = cullentIndex - 1;

        if (x < 0) return;

        cullentIndex = x;

        ShowDiscription(items[cullentIndex]);
    }

    void ShowDiscription(Base_PlayerItem item)
    {
        icon.sprite = item.sprite;

        descriptionArea_Knife.SetActive(false);
        descriptionArea_Treasure.SetActive(false);
        foreach (var button in KA_Icons)
        {
            button.gameObject.SetActive(false);
        }

        txt_ItemName.text = item._name;

        switch (item.element)
        {
            case Element.White:
                txt_Element.text = "白";
                break;

            case Element.Red:
                txt_Element.text = "赤";
                break;

            case Element.Blue:
                txt_Element.text = "青";
                break;

            case Element.Yellow:
                txt_Element.text = "黄";
                break;
        }

        txt_Rareity.text = item.rank.ToString();

        if (item is KnifeData knife)
        {
            txt_Type.text = "ナイフ";

            descriptionArea_Knife.SetActive(true);

            txt_BaseATK_Knife.text = $"基礎攻撃力：{knife.power}";
            txt_ElementATK_Knife.text = $"属性攻撃力：{knife.elementPower}";
            txt_Multiple_Knife.text = "";

            for (int i = 0; i < knife.abilities.Count; i++)
            {
                KA_Icons[i].Initialize(knife.abilities[i]);
            }
        } 
        else if (item is KnifeData_RunTime runtimeData)
        {
            txt_Type.text = "ナイフ";

            descriptionArea_Knife.SetActive(true);

            txt_BaseATK_Knife.text = $"基礎攻撃力：{runtimeData.power}";
            txt_ElementATK_Knife.text = $"属性攻撃力：{runtimeData.elementPower}";
            txt_Multiple_Knife.text = $"重複度：{runtimeData.count_Multiple}";

            for (int i = 0; i < runtimeData.abilities.Count; i++)
            {
                KA_Icons[i].Initialize(runtimeData.abilities[i]);
            }
        }
        else if (item is TreasureData treasure)
        {
            txt_Type.text = "秘宝";

            descriptionArea_Treasure.SetActive(true);

            description_Treasure.text = treasure.description;
        }


    }

    void ClosePanel()
    {
        body_Panel.SetActive(false);
    }
}
