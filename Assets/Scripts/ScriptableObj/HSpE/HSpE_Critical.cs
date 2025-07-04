using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHSpE", menuName = "Game Data/HSpE/Critical")]
public class HSpE_Critical : BaseHSpE
{
    private bool _critical; // �v���p�e�B�̒l��ێ����邽�߂̃v���C�x�[�g�t�B�[���h
    // ���������critical�Ɋ��܂��Ȃ��Ɩ������[�v�̃G���[�ɂȂ�

    public override bool critical
    {
        get { return _critical; }
        set {  _critical = value; }
    }

    // �m��
    public float probability;

    public override void OnHitSpecialEffect(Base_MobStatus status, Vector2 posi)
    {
        float x = Random.Range(0f, 1f);

        if (x <= probability)
        {
            critical = true;
        }
        else
        {
            critical = false;
        }
    }
}
