using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public abstract class Base_EnemyCtrler : MonoBehaviour
{
    [SerializeField] LayerMask targetLayer;

    [SerializeField] GameObject fx_Die;

    public Transform target {  get; protected set; }

    public EnemyStatus _enemyStatus { get; protected set; }

    public virtual void Initialize()
    {
        _enemyStatus = GetComponent<EnemyStatus>();

        target = PlayerController.Instance.transform;

        EnemyStatus.onDie
            .Where(x => x.status == _enemyStatus)
            .Subscribe(x =>
            {
                OnDie();

            }).AddTo(this);
    }

    // Update is called once per frame
     protected virtual void FixedUpdate()
    {
        if (_enemyStatus.actable == false) return;

        if(target == null) return;

        HandleAI();
    }

    protected abstract void HandleAI();

    protected virtual void OnDie()
    {
        Instantiate(fx_Die, transform.position, Quaternion.identity);

        Destroy(this.gameObject);
    }

}
