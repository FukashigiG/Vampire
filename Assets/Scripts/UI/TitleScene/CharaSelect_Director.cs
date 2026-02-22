using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;

public class CharaSelect_Director : SingletonMono<CharaSelect_Director>
{
    [SerializeField] GameObject prefab_Btn_CharaSelect;

    [SerializeField] GameObject bodyPanel;

    [SerializeField] Image BG_Image;
    [SerializeField] Image charaFullBodyArt;
    [SerializeField] Button btn_Close;
    [SerializeField] Button btn_GoButtle;
    [SerializeField] List<UI_PlayerItemButton> btns_Knife;
    [SerializeField] List<UI_PlayerItemButton> btns_Treasure;
    [SerializeField] Transform btnArea;
    [SerializeField] Transform KnifeArea;
    [SerializeField] Transform TreasureArea;
    [SerializeField] TextMeshProUGUI txt_CharaName;
    [SerializeField] TextMeshProUGUI txt_Element;
    [SerializeField] TextMeshProUGUI txt_ChataAbilityName;
    [SerializeField] TextMeshProUGUI txt_ChataAbilityDiscription;
    [SerializeField] List<TextMeshProUGUI> txts_Status;

    [Header("素材割り当て")]
    [SerializeField] Sprite BG_Red;
    [SerializeField] Sprite BG_Blue;
    [SerializeField] Sprite BG_Yellow;

    List<PlayerCharaData> charas = new List<PlayerCharaData>();

    public PlayerCharaData cullentSelected { get; private set; } = null;

    public void Initialize(Button btn_Open)
    {
        btn_Open.onClick.AddListener(OpenPanel);
        btn_Close.onClick.AddListener(ClosePanel);
        btn_GoButtle.onClick.AddListener(GoButtle);

        charas = Resources.LoadAll<PlayerCharaData>("GameDatas/PlayerChara").ToList();

        foreach (PlayerCharaData _chara in charas)
        {
            var obj = Instantiate(prefab_Btn_CharaSelect, btnArea);

            var btn = obj.GetComponent<CharaSelect_Button>();

            btn.Setup(_chara);

            btn._onClicked.Subscribe(_charaData =>
            {
                SetInfo(_charaData);

            }).AddTo(this);
        }

    }

    void SetInfo(PlayerCharaData data)
    {
        foreach(var btn in btns_Knife)
        {
            btn.onClicked.RemoveAllListeners();

            btn.gameObject.SetActive(false);
        }

        foreach(var btn in btns_Treasure)
        {
            btn.onClicked.RemoveAllListeners();

            btn.gameObject.SetActive(false);
        }

        cullentSelected = data;

        txt_CharaName.text = data._name;

        charaFullBodyArt.gameObject.SetActive(true);
        charaFullBodyArt.sprite = data.image_FullBody;

        switch (data.masteredElement)
        {
            case Element.Red:
                txt_Element.text = "赤";
                BG_Image.sprite = BG_Red;
                break;

            case Element.Blue:
                txt_Element.text = "青";
                BG_Image.sprite = BG_Blue;
                break;

            case Element.Yellow:
                txt_Element.text = "黄";
                BG_Image.sprite = BG_Yellow;
                break;

            case Element.White:
                txt_Element.text = "白";
                break;
        }

        txts_Status[0].text = data.hp.ToString();
        txts_Status[1].text = data.power.ToString();
        txts_Status[2].text = data.defense.ToString();
        txts_Status[3].text = data.moveSpeed.ToString();

        txt_ChataAbilityName.text = data.charaAbility.abilityName;
        txt_ChataAbilityDiscription.text = data.charaAbility.explanation;

        for (int i = 0; i < data.initialKnives.Count(); i++)
        {
            btns_Knife[i].gameObject.SetActive(true);

            btns_Knife[i].SetData(data.initialKnives[i]);

            int index = i;

            btns_Knife[i].onClicked.AddListener((x) =>
            {
                UI_ShowPlayerItemInfo.Instance.ShowPanel(data.initialKnives.ToList<Base_PlayerItem>(), index);
            });
        }

        for (int i = 0; i < data.initialTreasures.Count(); i++)
        {
            btns_Treasure[i].gameObject.SetActive(true);

            btns_Treasure[i].SetData(data.initialTreasures[i]);

            int index = i;

            btns_Treasure[i].onClicked.AddListener((x) =>
            {
                UI_ShowPlayerItemInfo.Instance.ShowPanel(data.initialTreasures.ToList<Base_PlayerItem>(), index);
            });
        }

        btn_GoButtle.interactable = true;
    }

    // パネルを開く際の処理
    void OpenPanel()
    {
        // 何も選択状態でなければ、立ち絵用objを隠し、各テキストを空欄にし、はじめるボタンを無効化
        if(cullentSelected == null)
        {
            charaFullBodyArt.gameObject.SetActive(false);

            txt_Element.text = "";

            txts_Status[0].text = "";
            txts_Status[1].text = "";
            txts_Status[2].text = "";
            txts_Status[3].text = "";

            txt_CharaName.text = "";

            txt_ChataAbilityName.text = "";
            txt_ChataAbilityDiscription.text = "";

            btn_GoButtle.interactable = false;
        }

        bodyPanel.SetActive(true);
    }

    void ClosePanel()
    {
        bodyPanel.SetActive(false);
    }

    void GoButtle()
    {
        if(cullentSelected == null) return;

        TitleSceneManager.Instance.GoButtle();
    }
}
