using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    // 攻撃を受け、ダメージをくらう処理のインターフェース

    bool GetAttack(int dmg, int elementDmg, Vector2 damagedPosi, bool isCritical = false, bool isIgnoreDefence = false);

    int TakeDamage(int dmg);
}
