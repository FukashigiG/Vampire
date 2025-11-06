using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    // 攻撃を受け、ダメージをくらう処理のインターフェース

    void GetAttack(int dmg, int elementDmg, Vector2 damagedPosi, bool isCritical, bool isIgnoreDefence);

    void TakeDamage(int dmg);
}
