using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDebuffable
{
    void MoveSpeedDebuff(float duration, float amount);
    void AttackDebuff(float duration, float amount);
    void DefenceDebuff(float duration, float amount);
}
