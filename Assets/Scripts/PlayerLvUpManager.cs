using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class PlayerLvUpManager : MonoBehaviour
{
    [SerializeField] GameObject button_Option;
    [SerializeField] GameObject buttonArea;

    [SerializeField] List<KnifeData> allKnifeData;

    private void Start()
    {
        //ナイフ追加画面のボタンが押された際に、それを検知し関数を実行
        Button_AddKnifeCtrler.clicked.Subscribe(xx => Choice(xx));
    }


    //パネルがActiveになったとき
    private void OnEnable()
    {
        int num_Option = Random.Range(2, 6);

        //２～５個のボタンを用意
        for (int i = 0; i < num_Option; i++)
        {
            //生成したボタンをxと置く
            var buttonObj =  Instantiate(button_Option, buttonArea.transform);

            //ランダムなナイフを選出
            var randomKnife = DrawingKnives();

            //コンポーネントの取得
            var buttonCtrler = buttonObj.GetComponent<Button_AddKnifeCtrler>();

            //ボタンに選出したナイフの情報を渡す
            buttonCtrler.SetInfo(randomKnife);

            //ボタンが押された際の処理はStart()にて記載済み
        }
    }

    void Choice(KnifeData knifeData)
    {
        //プレイヤーに抽選されたナイフの追加
        //流石にヤケクソ実装すぎるので修正必須
        GameObject.Find("Player").GetComponent<PlayerAttack>().AddKnife(knifeData);

        //buttonAreaの子オブジェクトを全削除
        foreach (Transform button in buttonArea.transform)
        {
            Destroy(button.gameObject);
        }

        //パネルを閉じる
        this.gameObject.SetActive(false);
    }

    //ナイフの抽選
    KnifeData DrawingKnives()
    {
        int x = Random.Range(0, allKnifeData.Count);

        var y = allKnifeData[x];

        return y;
    }
}
