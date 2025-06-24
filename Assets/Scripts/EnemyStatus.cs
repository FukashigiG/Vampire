using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : Base_MobStatus
{
    [SerializeField] EnemyData _enemyData;

    [SerializeField] GameObject damageTxt;

    float weight;

    protected override void Start()
    {
        base.Start();

        hitPoint = _enemyData.hp;
        defence = _enemyData.defense;
        weight = _enemyData.weight;
    }

    public override void TakeDamage(int a, Vector2 damagedPosi)
    {
        a -= defence;

        base.TakeDamage(a, damagedPosi);

        // �m�b�N�o�b�N
        var damageDir = (damagedPosi - (Vector2)transform.position).normalized;
        transform.Translate(damageDir * -1 * (1 / (1 + weight)));

        //�_���[�W�e�L�X�g���o������
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);

        var x = Instantiate(damageTxt, screenPoint, Quaternion.identity, GameObject.Find("Parent_DamageTxt").transform);

        x.GetComponent<DamageTxtCtrler>().Initialize(a);
    }
}
