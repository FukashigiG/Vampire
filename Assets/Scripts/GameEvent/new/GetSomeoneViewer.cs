using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Random = UnityEngine.Random;
using Unity.VisualScripting;


public class GetSomeoneViewer : SingletonMono<GetSomeoneViewer>
{
    // 武器・秘宝獲得発動時、その内容を表示するUI用スクリプト

    Animator animator;

    [field: SerializeField] public GameObject body_Panel { get; private set; }

    [SerializeField] Button[] buttons_Option;
    [SerializeField] Button button_Decide;
    [SerializeField] Button button_Skip;

    [SerializeField] Text txt_Type;
    [SerializeField] Text txt_ItemName;
    [SerializeField] Text txt_Rareity;
    [SerializeField] Text txt_Element;

    [SerializeField] GameObject descriptionArea_Knife;
    [SerializeField] Text txt_BaseATK_Knife;
    [SerializeField] Text txt_ElementATK_Knife;
    [SerializeField] Text txt_multipleCount;
    [SerializeField] Icon_KnifeAbility[] KA_Icons;

    [SerializeField] GameObject descriptionArea_Treasure;
    [SerializeField] Text description_Treasure;

    enum ItemType { knife, treasure}

    Dictionary<ItemType, int> itemTypeWeight = new Dictionary<ItemType, int>();
    int sum_TypeWeight = 0;
    Dictionary<Element, int> elementsWeight = new Dictionary<Element, int>();
    Dictionary<int, int> rankWeight = new Dictionary<int, int>();
    int sum_rankWeight = 0;

    Base_PlayerItem currentSelected;

    List<KnifeData> _cachedKnives;
    List<TreasureData> _cachedTreasures;

    List<Base_PlayerItem> lotteriedItems = new List<Base_PlayerItem>();

    private void Awake()
    {
        animator = GetComponent<Animator>();

        button_Decide.onClick.AddListener(Decision);
        button_Skip.onClick.AddListener(ClosePanel);

        LoadGameData();

        itemTypeWeight.Add(ItemType.knife, 70);
        itemTypeWeight.Add(ItemType.treasure, 30);
        foreach (var index in itemTypeWeight)
        {
            sum_TypeWeight += index.Value;
        }

        elementsWeight.Add(Element.Red, 10);
        elementsWeight.Add(Element.Blue, 10);
        elementsWeight.Add(Element.Yellow, 10);
        elementsWeight.Add(Element.White, 10);

        rankWeight.Add(1, 40);
        rankWeight.Add(2, 25);
        rankWeight.Add(3, 8);
        foreach (var index in rankWeight)
        {
            sum_rankWeight += index.Value;
        }
    }

    void LoadGameData()
    {
        // Resourcesフォルダ内のデータを取得
        _cachedKnives = Resources.LoadAll<KnifeData>("GameDatas/Knife").ToList();
        _cachedTreasures = Resources.LoadAll<TreasureData>("GameDatas/Treasure").ToList();
    }

    public void ShowEvent()
    {
        currentSelected = null;
        lotteriedItems.Clear();

        body_Panel.SetActive(true);

        foreach(var button  in buttons_Option)
        {
            button.gameObject.SetActive(true);

            button.onClick.RemoveAllListeners();

            Base_PlayerItem _item = GetRandomItem();

            if (_item == null)
            {
                // アイテムが取得できなかった場合,ボタンを隠す
                button.gameObject.SetActive(false); 
                continue;
            }

            lotteriedItems.Add(_item);

            button.GetComponent<Image>().sprite = _item.sprite;

            button.onClick.AddListener(() =>
            {
                currentSelected = _item;

                ShowDiscription(_item);
            });
        }

        descriptionArea_Knife.SetActive(false);
        descriptionArea_Treasure.SetActive(false);

        txt_ItemName.text = "";
        txt_Rareity.text = "";
        txt_Element.text = "";
        txt_Type.text = "";

        animator.SetTrigger("Anim");
    }

    void Decision()
    {
        if(currentSelected == null) return;

        if(currentSelected is KnifeData knife)
        {
            PlayerController.Instance._status.inventory.AddKnife(new KnifeData_RunTime(knife));
        }
        else if(currentSelected is TreasureData treasure)
        {
            PlayerController.Instance._status.inventory.AddTreasure(treasure);
        }

        ClosePanel();
    }

    void ShowDiscription(Base_PlayerItem item)
    {
        descriptionArea_Knife.SetActive(false);
        descriptionArea_Treasure.SetActive(false);
        foreach(var button in KA_Icons)
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

            for (int i = 0; i < knife.abilities.Count; i++)
            {
                KA_Icons[i].Initialize(knife.abilities[i]);
            }

            var isKnown = PlayerController.Instance._status.inventory.runtimeKnives.FirstOrDefault(x => x._name == knife._name);

            if (isKnown == null)
            {
                txt_multipleCount.text = "";
            }
            else
            {
                txt_multipleCount.text = $"重複度：{isKnown.count_Multiple} → {isKnown.count_Multiple + 1}";
            }
        }
        else if (item is TreasureData treasure)
        {
            txt_Type.text = "秘宝";

            descriptionArea_Treasure.SetActive(true);

            description_Treasure.text = treasure.description;
        }


    }

    Base_PlayerItem GetRandomItem()
    {
        ItemType targetType = Lottery_Type();
        int targetRank = Lottery_Rank();
        Element targetElement = Lottery_Element();

        List<Base_PlayerItem> candidates = new List<Base_PlayerItem>();

        switch (targetType)
        {
            case ItemType.knife:
                candidates.AddRange(_cachedKnives.Where(x => x.rank == targetRank && x.element == targetElement));
                break;

            case ItemType.treasure:

                HashSet<string> excludedIDs = new HashSet<string>(PlayerController.Instance._status.inventory.runtimeTreasure.Select(x => x._name));

                // Playerの持ってない秘宝のリストを作成
                List<TreasureData> availableTreasures = _cachedTreasures
                    .Where(treasureAsset => !excludedIDs.Contains(treasureAsset._name))
                    .ToList();

                candidates.AddRange(availableTreasures.Where(x => x.rank == targetRank && x.element == targetElement));
                break;

            default:
                candidates.AddRange(_cachedKnives.Where(x => x.rank == targetRank && x.element == targetElement));
                break;
        }

        if (candidates.Count == 0) return null;

        return candidates[Random.Range(0, candidates.Count)];
    }

    ItemType Lottery_Type()
    {
        int randomPoint = Random.Range(1, sum_TypeWeight + 1);

        int cullemt = 0;

        foreach (var index in itemTypeWeight)
        {
            cullemt += index.Value;

            if (cullemt >= randomPoint) return index.Key;
        }

        return ItemType.knife;
    }

    Element Lottery_Element()
    {
        Dictionary<Element, int> newElementsWeight = new Dictionary<Element, int>(elementsWeight);
        int sum_ElementWeight = 0;

        // キーのみを取り出したリストを作成
        List<Element> keys = newElementsWeight.Keys.ToList();

        // プレイヤーの得意属性なら比重を加算し、その合計を取得
        // Dictionaryを対象にforeachループさせると、コレクションの中身を変えられない仕様なのでListをかませる
        foreach (var key in keys)
        {
            if (PlayerController.Instance._status.masteredElements.Contains(key)) newElementsWeight[key] += 50;
            sum_ElementWeight += newElementsWeight[key];
        }

        int randomPoint = Random.Range(1, sum_ElementWeight + 1);

        int cullemt = 0;

        foreach (var index in newElementsWeight)
        {
            cullemt += index.Value;

            if (cullemt >= randomPoint)
            {
                return index.Key;
            }
        }

        return Element.White;
    }

    int Lottery_Rank()
    {
        int randomPoint = Random.Range(1, sum_rankWeight + 1);

        int cullemt = 0;

        foreach (var index in rankWeight)
        {
            cullemt += index.Value;

            if(cullemt >= randomPoint)
            {
                return index.Key;
            }
        }

        return 0;
    }

    void ClosePanel()
    {
        body_Panel.SetActive(false);
    }
}
