using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    void GetAttack(int dmg, Vector2 damagedPosi);

    void TakeDamage(int dmg, Vector2 damagedPosi);
}
