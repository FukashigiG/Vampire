using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;

// これ↓を記述しておくことで、SystemとUnityEngineでRanbomが競合しなくなる
using Random = UnityEngine.Random;

public class Eve_DriveKnife : Base_EventCtrler
{
    // 所持ナイフのうちランダムにピックされたいくらかの選択肢から
    // 一つを選び、破棄することで、何かが起こるイベント

    [SerializeField] GameObject button_Option;

    public override void Initialize()
    {
        base.Initialize();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        // プレイヤーの所持ナイフリストをコピー
        List<KnifeData_RunTime> list = new List<KnifeData_RunTime>(PlayerController.Instance._status.inventory.runtimeKnives);

        // リストのシャッフル
        list = list.OrderBy(a => Guid.NewGuid()).ToList();

        // 選択肢の総数を決定
        int num_Options = Random.Range(3, 5);

        for (int i = 0; i < num_Options; i++)
        {
            if (list.Count <= 0) break;

            var KnifeData = list[0];

            GameObject button = Instantiate(button_Option, buttonArea);

            var buttonCtrl = button.GetComponent<Button_Knife>();

            buttonCtrl.Initialize(KnifeData);

            buttonCtrl.clicked.Subscribe(xx => Choice(xx)).AddTo(buttonCtrl);

            list.Remove(KnifeData);
        }
    }

    void Choice(KnifeData_RunTime knifeData)
    {
        PlayerController.Instance._status.inventory.RemoveKnife(knifeData);

        TrigggerRandomAction();

        this.gameObject.SetActive(false);
    }

    void TrigggerRandomAction()
    {
        int x = Random.Range(0, 1);

        switch(x)
        {
            case 0:
                EnemySpawner.Instance.SpawnBoss();
                break;
        }
    }
}
