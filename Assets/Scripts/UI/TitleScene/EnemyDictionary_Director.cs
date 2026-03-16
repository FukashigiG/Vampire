using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine.UI;
using TMPro;


public class EnemyDictionary_Director : SingletonMono<EnemyDictionary_Director>
{
    [SerializeField] GameObject prefab_Btn;

    [SerializeField] GameObject bodyPanel;

    Button btn_Open;
    [SerializeField] Button btn_Close;
    [SerializeField] Transform btnArea;
    [SerializeField] TextMeshProUGUI txt_EnemyName;
    [SerializeField] TextMeshProUGUI txt_EnemyType;
    [SerializeField] TextMeshProUGUI txt_EnemyModify;
    [SerializeField] TextMeshProUGUI txt_EnemyDiscription;
    [SerializeField] Image image_Enemy;
    [SerializeField] List<TextMeshProUGUI> txts_Status;
    [SerializeField] List<Icon_ESA> icons_ESA;

    List<Button> buttons;

    float modifire = 1f;

    public void Initialize(Button _btn_Open, List<EnemyData> enemies, float _modifire = 1f, int index = -1)
    {
        modifire = _modifire;

        txt_EnemyModify.text = $"(x {modifire.ToString("F2")})";

        foreach(Transform child in btnArea)
        {
            Destroy(child.gameObject);
        }

        // 画面開閉登録
        btn_Open?.onClick.RemoveAllListeners();
        btn_Open = _btn_Open;
        btn_Open.onClick.AddListener(OpenPanel);
        btn_Close.onClick.RemoveAllListeners();
        btn_Close.onClick.AddListener(ClosePanel);

        // 全データ取得
        // やっぱ先にリストを用意してもらう形にしよう
        //enemies = Resources.LoadAll<EnemyData>("GameDatas/Enemy").ToList();

        // それぞれのデータの分だけボタンを用意、初期化
        foreach (EnemyData enemy in enemies)
        {
            var obj =  Instantiate(prefab_Btn, btnArea);

            var btn = obj.GetComponent<EnemyDictionary_Button>();

            btn.Setup(enemy);

            btn._onClicked.Subscribe(_enemyData =>
            {
                SetInfo(_enemyData);

            }).AddTo(this);
        }

        if(index >  -1 && index < enemies.Count) SetInfo(enemies[index]);
    }

    // 渡された敵の情報を表示
    void SetInfo(EnemyData data)
    {
        foreach (var icon in icons_ESA)
        {
            icon.gameObject.SetActive(false);
        }

        txt_EnemyName.text = data._name;

        switch (data.actType)
        {
            case EnemyData.EnemyActType.Infight:
                txt_EnemyType.text = "インファイター";
                break;

            case EnemyData.EnemyActType.Shooter:
                txt_EnemyType.text = "シューター";
                break;

            case EnemyData.EnemyActType.Fielder:
                txt_EnemyType.text = "フィールダー";
                break;

            case EnemyData.EnemyActType.BigBoss:
                txt_EnemyType.text = "ビッグボス";
                break;
        }

        txt_EnemyDiscription.text = data.description;

        txts_Status[0].text = "HP : " + (int)(data.hp * modifire);
        txts_Status[1].text = "攻撃力 : " + (int)(data.power * modifire);
        txts_Status[2].text = "防御力 : " + (int)(data.defense * modifire);
        txts_Status[3].text = "移動速度 : " + (int)(data.moveSpeed * modifire);

        for (int i = 0; i < data.statusAbilities.Count; i++)
        {
            icons_ESA[i].Initialize(data.statusAbilities[i]);
        }

        image_Enemy.gameObject.SetActive(true);

        image_Enemy.sprite = data.sprite;
    }

    void OpenPanel()
    {
        bodyPanel.SetActive(true);

        txt_EnemyName.text = "";
        txt_EnemyType.text = "";
        txt_EnemyDiscription.text = "";
        txts_Status[0].text = "HP : ";
        txts_Status[1].text = "攻撃力 : ";
        txts_Status[2].text = "防御力 : ";
        txts_Status[3].text = "移動速度 : ";

        image_Enemy.gameObject.SetActive(false);
    }

    void ClosePanel()
    {
        bodyPanel.SetActive(false);
    }
}
