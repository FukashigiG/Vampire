using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine.UI;


public class EnemyDictionary_Director : SingletonMono<EnemyDictionary_Director>
{
    [SerializeField] GameObject prefab_Btn;

    [SerializeField] GameObject bodyPanel;

    [SerializeField] Button btn_Close;
    [SerializeField] Transform btnArea;
    [SerializeField] Text txt_EnemyName;
    [SerializeField] Text txt_EnemyType;
    [SerializeField] Text txt_EnemyDiscription;
    [SerializeField] List<Text> txts_Status;
    [SerializeField] List<Icon_ESA> icons_ESA;

    List<EnemyData> enemies = new List<EnemyData>();

    public void Initialize(Button btn_Open)
    {
        // 画面開閉登録
        btn_Open.onClick.AddListener(OpenPanel);
        btn_Close.onClick.AddListener(ClosePanel);

        // 全データ取得
        enemies = Resources.LoadAll<EnemyData>("GameDatas/Enemy").ToList();

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

            case EnemyData.EnemyActType.BigBoss:
                txt_EnemyType.text = "ビッグボス";
                break;
        }

        txt_EnemyDiscription.text = data.description;

        txts_Status[0].text = "HP : " + data.hp;
        txts_Status[1].text = "攻撃力 : " + data.power;
        txts_Status[2].text = "防御力 : " + data.defense;
        txts_Status[3].text = "移動速度 : " + data.moveSpeed;

        for (int i = 0; i < data.statusAbilities.Count; i++)
        {
            icons_ESA[i].Initialize(data.statusAbilities[i]);
        }
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
