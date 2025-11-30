using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Random = UnityEngine.Random;


public class GetSomeoneViewer : SingletonMono<GetSomeoneViewer>
{
    // 武器・秘宝獲得発動時、その内容を表示するUI用スクリプト

    [field: SerializeField] public GameObject body_Panel { get; private set; }

    [SerializeField] Button[] buttons_Option;
    [SerializeField] Button button_Decide;

    [SerializeField] Text txt_Type;
    [SerializeField] Text txt_ItemName;

    enum ItemType { knife, treasure}

    Base_PlayerItem currentSelected;

    List<KnifeData> _cachedKnives;
    List<TreasureData> _cachedTreasures;

    private void Awake()
    {
        button_Decide.onClick.AddListener(Decision);

        LoadGameData();
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

        body_Panel.SetActive(true);

        foreach(var button  in buttons_Option)
        {
            button.onClick.RemoveAllListeners();

            Base_PlayerItem _item = GetRandomItem();

            if (_item == null)
            {
                // アイテムが取得できなかった場合,ボタンを隠す
                button.gameObject.SetActive(false); 
                continue;
            }

            button.GetComponent<Image>().sprite = _item.sprite;

            button.onClick.AddListener(() =>
            {
                currentSelected = _item;

                ShowDiscription(_item);
            });
        }
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
        txt_ItemName.text = item._name;

        if (item is KnifeData knife)
        {
            txt_Type.text = "ナイフ";
        } 
        else if(item is TreasureData treasure)
        {
            txt_Type.text = "秘宝";
        }


    }

    Base_PlayerItem GetRandomItem()
    {
        ItemType targetType = Lottery_Type();
        int targetRank = Lottery_Rank();
        Debug.Log($"{targetType}, {targetRank}");

        List<Base_PlayerItem> candidates = new List<Base_PlayerItem>();

        switch (targetType)
        {
            case ItemType.knife:
                candidates.AddRange(_cachedKnives.Where(x => x.rank == targetRank));
                break;

            case ItemType.treasure:
                candidates.AddRange(_cachedTreasures.Where(x => x.rank == targetRank));
                break;

            default:
                candidates.AddRange(_cachedKnives.Where(x => x.rank == targetRank));
                break;
        }

        return candidates[Random.Range(0, candidates.Count)];
    }

    ItemType Lottery_Type()
    {
        int[] weight_Type = { 70, 30 };

        int sum = 100;

        int randomPoint = Random.Range(1, sum + 1);

        int cullemt = 0;

        for (int i = 0; i < weight_Type.Length; i++)
        {
            cullemt += weight_Type[i];

            if(cullemt >= randomPoint)
            {
                switch (i)
                {
                    case 0:
                        return ItemType.knife;

                    case 1:
                        return ItemType.treasure;
                }
            }
        }

        return ItemType.knife;
    }

    int Lottery_Rank()
    {
        int[] weight_Rank = { 40, 25, 10 };

        int sum = 75;

        int randomPoint = Random.Range(1, sum + 1);

        int cullemt = 0;

        for (int i = 0; i < weight_Rank.Length; i++)
        {
            cullemt += weight_Rank[i];

            if (cullemt >= randomPoint)
            {
                switch (i)
                {
                    case 0:
                        return 1;

                    case 1:
                        return 2;

                    case 2:
                        return 3;
                }
            }
        }

        return 0;
    }

    void ClosePanel()
    {
        body_Panel.SetActive(false);
    }
}
