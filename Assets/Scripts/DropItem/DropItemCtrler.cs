using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class DropItemCtrler : MonoBehaviour
{
    enum EventEnum
    {
        getKnife, getTreasure, driveKnife
    }

    [SerializeField] EventEnum eventEnum;

    [SerializeField] float lifeSpan;

    float elapsedTime;

    public IObservable<Unit> onDestroy => subject_OnDestroy;
    Subject<Unit> subject_OnDestroy = new Subject<Unit>();

    private void Awake()
    {
        MiniMapController.Instance.NewItemInstance(this);
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > lifeSpan) Destroy(this.gameObject);
    }

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

    private void OnDestroy()
    {
        subject_OnDestroy.OnNext(Unit.Default);
    }
}
