using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Base_EnemyCtrler : MonoBehaviour
{
    [SerializeField] LayerMask targetLayer;

    protected Transform target;

    protected EnemyStatus _enemyStatus;

    protected virtual void Awake()
    {
        _enemyStatus = GetComponent<EnemyStatus>();
    }

    // Start is called before the first frame update
    void Start()
    {
        target = PlayerController.Instance.transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_enemyStatus.actable == false) return;

        if(target == null) return;

        HandleAI();
    }

    protected abstract void HandleAI();


    private void OnCollisionStay2D(Collision2D collision)
    {
        // もし当たったものがダメージを受けるものだったらダメージを与える
        if (collision.gameObject.TryGetComponent(out Base_MobStatus ms))
        {
            ms?.GetAttack(_enemyStatus.power, 0, transform.position);

            //Instantiate(knifeData.hitEffect, transform.position, Quaternion.identity);
        }
    }

}
