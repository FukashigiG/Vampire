using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharpKnifeCtrler : Base_KnifeCtrler
{

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        // ���������������񂪃_���[�W���󂯂���̂�������_���[�W
        if (collision.TryGetComponent(out IDamagable i_d))
        {
            i_d.TakeDamage(power, transform.position);

            // �������������̃I�u�W�F�N�g��Destroy���鏈�����폜�������߁A�ђʂ���悤�ɂȂ��Ă�
        }
    }
}
