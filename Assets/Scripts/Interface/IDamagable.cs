using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    // �U�����󂯁A�_���[�W�����炤�����̃C���^�[�t�F�[�X

    void GetAttack(int dmg, Vector2 damagedPosi);

    void TakeDamage(int dmg, Vector2 damagedPosi);
}
