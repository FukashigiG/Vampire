using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Base_EnemyCtrler : MonoBehaviour
{
    [SerializeField] LayerMask targetLayer;

    public Transform target {  get; protected set; }

    protected EnemyStatus _enemyStatus;

    protected virtual void Awake()
    {
        _enemyStatus = GetComponent<EnemyStatus>();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
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
