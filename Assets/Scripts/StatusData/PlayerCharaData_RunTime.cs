using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharaData_RunTime
{
    public Sprite sprite;

    public string _name;

    public int hp;
    public int power;
    public int defense;
    public int moveSpeed;
    public float luck;
    public float eyeSight;

    public KnifeData[] initialKnives;
    public Base_TreasureData[] initialTreasures;

    public Element masteredElement;

    public GameObject prefab;

    public PlayerCharaData_RunTime(PlayerCharaData data)
    {
        sprite = data.sprite;
        _name = data.name;
        hp = data.hp;
        power = data.power;
        defense = data.defense;
        moveSpeed = data.moveSpeed;
        luck = data.luck;
        eyeSight = data.eyeSight;
        initialKnives = data.initialKnives;
        initialTreasures = data.initialTreasures;
        masteredElement = data.masteredElement;
        prefab = data.prefab; 
    }
}
