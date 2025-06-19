using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLvUpManager : MonoBehaviour
{
    [SerializeField] GameObject button_Option;
    [SerializeField] GameObject buttonArea;

    [SerializeField] List<KnifeData> allKnifeData;

    private void OnEnable()
    {
        int num_Option = Random.Range(2, 6);

        for (int i = 0; i < num_Option; i++)
        {
            //生成したボタンをxと置く
            var x =  Instantiate(button_Option, buttonArea.transform);

            var y = DrawingKnives();

            //xのbuttonコンポーネントのonClickに反応してChoice関数を実行せよ
            x.GetComponent<Button>().onClick.AddListener(() => Choice(y));
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
