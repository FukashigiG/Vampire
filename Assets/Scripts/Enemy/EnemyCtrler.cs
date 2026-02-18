using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Base_EnemyCtrler : MonoBehaviour
{
    [SerializeField] LayerMask targetLayer;

    [SerializeField] GameObject fx_Die;

    public Transform target {  get; protected set; }

    public EnemyStatus _enemyStatus { get; protected set; }

    public virtual void Initialize()
    {
        _enemyStatus = GetComponent<EnemyStatus>();

        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = _enemyStatus._enemyData.sprite;

        target = PlayerController.Instance.transform;

        _enemyStatus.onDie.Subscribe(x =>
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

        DropItems();

        Destroy(this.gameObject);
    }

    void DropItems()
    {
        int randomPoint = 0;

        foreach(var item in _enemyStatus._enemyData.dropItems)
        {
            randomPoint = Random.Range(1, 101);

            if(randomPoint < item.dropRate_Parcentage)
            {
                Instantiate(item.prefab, transform.position, Quaternion.identity);
            }
        }
    }
}
