using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtrler_Fielder : Base_EnemyCtrler
{
    // フィールダー型の敵の挙動

    [SerializeField] GameObject prefab_Field;

    public EP_Field field { get; private set; }

    public override void Initialize()
    {
        base.Initialize();

        // フィールドオブジェクトを生成
        var fieldObj = Instantiate(prefab_Field, this.transform);

        // 変数に代入
        var field = fieldObj.GetComponent<EP_Field>();

        // 初期化
        field.Initialize_Field(_enemyStatus._enemyData.fieldLogic, _enemyStatus._enemyData.radius_FieldSize, 0, _enemyStatus.power);
    }

    protected override void HandleAI()
    {
        // シンプルに接近する

        //float distance = (target.position - this.transform.position).magnitude;

        Vector2 dir = (target.position - this.transform.position).normalized;

        this.transform.Translate(dir * _enemyStatus.moveSpeed / 10f * Time.fixedDeltaTime);
    }

    // フィールダーはプレイヤーと触れてもダメージを与えない
}
