using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventItemCtrler : MonoBehaviour
{
    enum EventEnum
    {
        getKnife, getTreasure, driveKnife
    }

    [SerializeField] EventEnum eventEnum;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (eventEnum)
        {
            case EventEnum.getKnife:
                GameEventDirector.Instance.TriggerEvent(GameEventDirector.Events.getKnife);
                break;

            case EventEnum.getTreasure:
                GameEventDirector.Instance.TriggerEvent(GameEventDirector.Events.getTreasure);
                break;

            case EventEnum.driveKnife:
                GameEventDirector.Instance.TriggerEvent(GameEventDirector.Events.driveKnife);
                break;
        }

        Destroy(this.gameObject);
    }
}
