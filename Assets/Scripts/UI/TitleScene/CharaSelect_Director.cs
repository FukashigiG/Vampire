using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class CharaSelect_Director : MonoBehaviour
{
    [SerializeField] GameObject prefab_Btn;

    [SerializeField] GameObject bodyPanel;
    [SerializeField] Button btn_Open;
    [SerializeField] Button btn_Close;
    [SerializeField] Transform btnArea;
    [SerializeField] Text txt_CharaName;
    [SerializeField] Text txt_Element;
    [SerializeField] Text txt_EnemyDiscription;
    [SerializeField] List<Text> txts_Status;

    List<PlayerCharaData> charas = new List<PlayerCharaData>();

    private void Awake()
    {
        btn_Open.onClick.AddListener(OpenPanel);
        btn_Close.onClick.AddListener(ClosePanel);

        charas = Resources.LoadAll<PlayerCharaData>("GameDatas/PlayerChara").ToList();

        foreach (PlayerCharaData _chara in charas)
        {
            var obj = Instantiate(prefab_Btn, btnArea);

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
        txt_CharaName.text = data._name;

        switch (data.masteredElement)
        {
            case Element.Red:
                txt_Element.text = "ê‘";
                break;

            case Element.Blue:
                txt_Element.text = "ê¬";
                break;

            case Element.Yellow:
                txt_Element.text = "â©";
                break;

            case Element.White:
                txt_Element.text = "îí";
                break;
        }

        txts_Status[0].text = "HP : " + data.hp;
        txts_Status[1].text = "çUåÇóÕ : " + data.power;
        txts_Status[2].text = "ñhå‰óÕ : " + data.defense;
        txts_Status[3].text = "à⁄ìÆë¨ìx : " + data.moveSpeed;
    }

    void OpenPanel()
    {
        bodyPanel.SetActive(true);
    }

    void ClosePanel()
    {
        bodyPanel.SetActive(false);
    }
}
