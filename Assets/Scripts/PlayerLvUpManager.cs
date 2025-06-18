using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLvUpManager : MonoBehaviour
{
    [SerializeField] GameObject button_Option;
    [SerializeField] GameObject buttonArea;

    private void OnEnable()
    {
        int num_Option = Random.Range(2, 6);

        for (int i = 0; i < num_Option; i++)
        {
            //生成したボタンをxと置く
            var x =  Instantiate(button_Option, buttonArea.transform);

            //xのbuttonコンポーネントのonClickに反応してChoice関数を実行せよ
            x.GetComponent<Button>().onClick.AddListener(Choice);
        }
    }

    void Choice()
    {
        //List<GameObject> _list = new List<GameObject>();

        //buttonAreaの子オブジェクトを全削除
        foreach (Transform button in buttonArea.transform)
        {
            Destroy(button.gameObject);
        }

        //foreach(GameObject button in _list)
        //{
        //    Destroy(button);
        //}

        //パネルを閉じる
        this.gameObject.SetActive(false);
    }
}
