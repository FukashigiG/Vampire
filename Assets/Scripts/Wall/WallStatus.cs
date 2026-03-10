using UnityEngine;

public class WallStatus : MonoBehaviour, IDamagable
{
    // IDamagableを構成する関数のガワだけ所持しておく

    // 攻撃を受ける処理
    public virtual bool GetAttack(int damagePoint, int elementPoint, Vector2 damagedPosi, bool isCritical = false, bool isIgnoreDefence = false)
    {
        return true;
    }

    // ダメージ処理
    public virtual int TakeDamage(int value)
    {
        return 0;
    }
}
