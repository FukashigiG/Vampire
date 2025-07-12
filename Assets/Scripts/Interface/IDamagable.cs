using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    // 攻撃を受け、ダメージをくらう処理のインターフェース

    void GetAttack(int dmg, Vector2 damagedPosi);

    void TakeDamage(int dmg, Vector2 damagedPosi);
}
