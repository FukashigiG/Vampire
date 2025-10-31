using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeData_RunTime
{
    public Sprite sprite { get; private set; }
    public Sprite sprite_Defolme { get; private set; }
    public Color color { get; private set; }

    public string _name { get; private set; }

    public int rarity { get; private set; }

    public Element element;

    public int power;
    public int elementPower;

    public GameObject prefab { get; private set; }

    public GameObject hitEffect;

    // ��������ƃG�f�B�^��Œ����e�L�X�g�������₷���Ȃ�
    [Multiline(6)] public string description;

    // �������
    public List<BaseHSpE> specialEffects;

    // �����^�C���p�łȂ��i�C�t�f�[�^�����ɂ���R���X�g���N�^
    public KnifeData_RunTime(KnifeData data)
    {
        sprite = data.sprite;
        sprite_Defolme = data.sprite_Defolme;
        color = data.color;
        _name = data._name;
        rarity = data.rarity;
        element = data.element;
        power = data.power;
        elementPower = data.elementPower;
        prefab = data.prefab;
        hitEffect = data.hitEffect;
        description = data.description;
        specialEffects = data.specialEffects;
    }

    // ���̃N���X�����ɂ���R���X�g���N�^
    public KnifeData_RunTime(KnifeData_RunTime data)
    {
        sprite = data.sprite;
        sprite_Defolme = data.sprite_Defolme;
        color = data.color;
        _name = data._name;
        rarity = data.rarity;
        element = data.element;
        power = data.power;
        elementPower = data.elementPower;
        prefab = data.prefab;
        hitEffect = data.hitEffect;
        description = data.description;
        specialEffects = data.specialEffects;
    }
}
