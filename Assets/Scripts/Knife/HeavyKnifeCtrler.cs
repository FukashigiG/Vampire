using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyKnifeCtrler : Base_KnifeCtrler
{
    [SerializeField] LayerMask targetLayer;

    [SerializeField] float radius;

    protected override void Start()
    {
        base.Start();

        //���͈͓��̓G��z��Ɋi�[
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, targetLayer);

        // ���X�g�ɂ���G���ꂼ��ɑ΂���
        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent(out IDamagable i_d))
            {
                // power�̔����̃_���[�W��^����
                i_d.TakeDamage((int)(power / 2), transform.position);
            }
        }
    }
}
