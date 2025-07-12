using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/Treasure Data/demo")]
public class demoTreasure : Base_TreasureData
{
    // �������Ă���ԁA�h��͂��㏸������

    public int amount;

    public override void OnAdd(PlayerStatus status)
    {
        status.defence += amount;

        Debug.Log(status.defence);
    }

    public override void OnRemove(PlayerStatus status)
    {
        status.defence -= amount;
    }

    public override void SubscribeToEvent()
    {

    }
}
