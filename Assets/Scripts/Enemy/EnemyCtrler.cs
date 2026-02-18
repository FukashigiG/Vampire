using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Base_EnemyCtrler : MonoBehaviour
{
    [SerializeField] SpriteRenderer bodyRenderer;

    [SerializeField] LayerMask targetLayer;

    [SerializeField] GameObject fx_Die;

    public Transform target {  get; protected set; }

    public EnemyStatus _enemyStatus { get; protected set; }

    // 初期化処理
    public virtual void Initialize()
    {
        _enemyStatus = GetComponent<EnemyStatus>();

        bodyRenderer.sprite = _enemyStatus._enemyData.sprite;

        target = PlayerController.Instance.transform;

        _enemyStatus.onDie.Subscribe(x =>
            {
                OnDie();

            }).AddTo(this);
    }

    // fixedUpdateのタイミングでHandleAIを実行する
     protected virtual void FixedUpdate()
    {
        if (_enemyStatus.actable == false) return;

        if(target == null) return;

        HandleAI();
    }

    // 毎フレームごとの処理は継承先が実装する
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
