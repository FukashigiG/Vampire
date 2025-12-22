using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Base_EnemyCtrler : MonoBehaviour
{
    [SerializeField] LayerMask targetLayer;

    public Transform target {  get; protected set; }

    public EnemyStatus _enemyStatus { get; protected set; }

    public virtual void Initialize()
    {
        _enemyStatus = GetComponent<EnemyStatus>();

        target = PlayerController.Instance.transform;
    }

    // Update is called once per frame
     protected virtual void FixedUpdate()
    {
        if (_enemyStatus.actable == false) return;

        if(target == null) return;

        HandleAI();
    }

    protected abstract void HandleAI();

}
