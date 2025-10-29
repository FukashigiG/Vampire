using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_EnemyCtrler : MonoBehaviour
{
    [SerializeField] LayerMask targetLayer;

    protected Transform target;

    protected EnemyStatus _enemyStatus;

    protected virtual void Awake()
    {
        _enemyStatus = GetComponent<EnemyStatus>();
    }

    // Start is called before the first frame update
    void Start()
    {
        target = PlayerController.Instance.transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_enemyStatus.actable == false) return;

        if(target == null) return;

        Vector2 dir = (target.position - this.transform.position).normalized;

        transform.Translate(dir * _enemyStatus.moveSpeed / 10f * Time.fixedDeltaTime);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // ���������������̂��_���[�W���󂯂���̂�������_���[�W��^����
        if (collision.gameObject.TryGetComponent(out Base_MobStatus ms))
        {
            /*
            bool shouldDestroyThis = true;

            // �i�C�t�ɓ���\�͂��ݒ肳��Ă����ꍇ�̏���
            foreach (var SpEffect in knifeData.specialEffects)
            {
                if (SpEffect != null)
                {
                    // �q�b�g���̓��ꏈ�������s
                    SpEffect.OnHitSpecialEffect(ms, transform.position, knifeData);

                    // �ђʂ�������Ă���Ȃ�
                    if (SpEffect.dontDestroyBullet == true) shouldDestroyThis = false;
                    // �h�䖳����������Ă���Ȃ�
                    if (SpEffect.ignoreDefence == true) power += ms.defence / 4; // �h��͕�����悹���邱�ƂŎ�������
                    // �N���e�B�J���Ȃ�
                    if (SpEffect.critical == true) power *= 2;
                }
            }
            */

            ms?.GetAttack(_enemyStatus.power, transform.position);

            //Instantiate(knifeData.hitEffect, transform.position, Quaternion.identity);
        }
    }

}
