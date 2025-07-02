using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : Base_MobStatus
{
    [SerializeField] EnemyData _enemyData;

    [SerializeField] GameObject damageTxt;

    protected override void Start()
    {
        base.Start();

        hitPoint = _enemyData.hp;
        defence = _enemyData.defense;
        weight = _enemyData.weight;
        moveSpeed = _enemyData.moveSpeed;
    }

    public override void TakeDamage(int a, Vector2 damagedPosi)
    {
        a -= defence;

        base.TakeDamage(a, damagedPosi);

        //ダメージテキストを出す処理
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);

        var x = Instantiate(damageTxt, screenPoint, Quaternion.identity, GameObject.Find("Parent_DamageTxt").transform);

        x.GetComponent<DamageTxtCtrler>().Initialize(a);
    }
}
