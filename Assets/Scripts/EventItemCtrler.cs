using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventItemCtrler : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameEventDirector.Instance.TriggerEvent(GameEventDirector.Events.driveKnife);

        Destroy(this.gameObject);
    }
}
