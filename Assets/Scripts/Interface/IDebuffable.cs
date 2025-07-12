using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDebuffable
{
    // �f�o�t�����̃C���^�[�t�F�[�X

    void MoveSpeedDebuff(float duration, float amount);
    void PowerDebuff(float duration, float amount);
    void DefenceDebuff(float duration, float amount);
    void Blaze(float duration);
    void Freeze(float duration);
}
