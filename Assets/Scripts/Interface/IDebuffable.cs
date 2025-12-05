public interface IDebuffable
{
    // デバフ処理のインターフェース

    void MoveSpeedDebuff(float duration, float amount);
    void PowerDebuff(float duration, float amount);
    void DefenceDebuff(float duration, float amount);
    void Blaze(float duration);
    void Freeze(float duration);
}
