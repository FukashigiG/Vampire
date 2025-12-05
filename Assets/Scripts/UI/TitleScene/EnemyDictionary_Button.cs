using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDictionary_Button : MonoBehaviour
{
    [SerializeField] Text txt_Btn;

    EnemyData enemyData;

    Subject<EnemyData> subject_OnClicked = new Subject<EnemyData>();
    public IObservable<EnemyData> _onClicked => subject_OnClicked;

    public void Setup(EnemyData _enemyData)
    {
        enemyData = _enemyData;

        txt_Btn.text = enemyData._name;

        GetComponent<Button>().onClick.AddListener(() =>
        {
            subject_OnClicked.OnNext(enemyData);
        });
    }
}
