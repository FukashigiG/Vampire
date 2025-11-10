using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;

public class MiniMapController : SingletonMono<MiniMapController>
{ 
    [SerializeField] GameObject enemyIcon;
    [SerializeField] GameObject itemIcon;

    [SerializeField] float mapScale;

    Dictionary<Base_MobStatus, RectTransform> enemyDictionary = new Dictionary<Base_MobStatus, RectTransform>();
    Dictionary<DropItemCtrler, RectTransform> itemDictionary = new Dictionary<DropItemCtrler, RectTransform>();

    [SerializeField] Transform playerTransform;

    void Start()
    {
        // いずれかの敵が死んだとき
        EnemyStatus.onDie.Subscribe(x =>
        {
            // そいつがdictionaryに登録されてるやつなら、
            if (enemyDictionary.ContainsKey(x.status))
            {
                // アイコンを破壊
                Destroy(enemyDictionary[x.status].gameObject);

                // 辞書から削除
                enemyDictionary.Remove(x.status);
            }

        }).AddTo(this);
    }

    
    void Update()
    {
        foreach(var enemy  in enemyDictionary)
        {
            var enemyTransform = enemy.Key.transform;

            // 1. プレイヤーから敵への相対位置を計算
            Vector2 relativePos = enemyTransform.position - playerTransform.position;

            // 2. 縮尺をかけて、ミニマップ上の座標に変換
            Vector2 minimapPos = relativePos * mapScale;

            // アイコンの座標を更新
            enemy.Value.anchoredPosition = minimapPos;
        }

        foreach (var item in itemDictionary)
        {
            var itemTransform = item.Key.transform;

            // 1. プレイヤーから敵への相対位置を計算
            Vector2 relativePos = itemTransform.position - playerTransform.position;

            // 2. 縮尺をかけて、ミニマップ上の座標に変換
            Vector2 minimapPos = relativePos * mapScale;

            // アイコンの座標を更新
            item.Value.anchoredPosition = minimapPos;
        }
    }

    // 新しく生成された敵オブジェクトに対して
    public void NewEnemyInstance(EnemyStatus enemyStatus)
    {
        // アイコンを生成
        GameObject icon = Instantiate(enemyIcon, this.transform);

        // その敵とアイコンを紐づけ
        enemyDictionary.Add(enemyStatus, icon.GetComponent<RectTransform>());
    }

    // 新しく生成された敵オブジェクトに対して
    public void NewItemInstance(DropItemCtrler dropItemCtrler)
    {
        // アイコンを生成
        GameObject icon = Instantiate(itemIcon, this.transform);

        // その敵とアイコンを紐づけ
        itemDictionary.Add(dropItemCtrler, icon.GetComponent<RectTransform>());

        // そのアイテムが消えた時の処理も登録しておく
        dropItemCtrler.onDestroy.Subscribe(x =>
        {
            // アイコンを破壊
            Destroy(itemDictionary[dropItemCtrler].gameObject);

            // 辞書から削除
            itemDictionary.Remove(dropItemCtrler);

        }).AddTo(this);
    }
}
