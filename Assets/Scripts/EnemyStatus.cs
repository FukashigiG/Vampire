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

        GetComponent<SpriteRenderer>().sprite = _enemyData.sprite;
    }

    public void Initialize(float multiplier)
    {
        maxHP = (int)((float)_enemyData.hp * multiplier);
        hitPoint = maxHP;

        base_Power = (int)(_enemyData.power * multiplier);
        base_Defence = (int)(_enemyData.defense * multiplier);
        base_MoveSpeed = (int)(_enemyData.moveSpeed * multiplier);
    }

    public override void TakeDamage(int a, Vector2 damagedPosi)
    {
        base.TakeDamage(a, damagedPosi);

        //�_���[�W�e�L�X�g���o������
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);

        var x = Instantiate(damageTxt, screenPoint, Quaternion.identity, GameObject.Find("Parent_DamageTxt").transform);

        x.GetComponent<DamageTxtCtrler>().Initialize((int)a);
    }
}
