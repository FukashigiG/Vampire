using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeData_RunTime
{
    public Sprite sprite { get; private set; }

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

    public KnifeData_RunTime(KnifeData data)
    {
        sprite = data.sprite;
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
