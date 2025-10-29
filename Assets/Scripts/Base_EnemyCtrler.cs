using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_EnemyCtrler : MonoBehaviour
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

        Vector2 dir = (target.position - this.transform.position).normalized;

        transform.Translate(dir * _enemyStatus.moveSpeed / 10f * Time.fixedDeltaTime);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // もし当たったものがダメージを受けるものだったらダメージを与える
        if (collision.gameObject.TryGetComponent(out Base_MobStatus ms))
        {
            /*
            bool shouldDestroyThis = true;

            // ナイフに特殊能力が設定されていた場合の処理
            foreach (var SpEffect in knifeData.specialEffects)
            {
                if (SpEffect != null)
                {
                    // ヒット時の特殊処理を実行
                    SpEffect.OnHitSpecialEffect(ms, transform.position, knifeData);

                    // 貫通が許可されているなら
                    if (SpEffect.dontDestroyBullet == true) shouldDestroyThis = false;
                    // 防御無視が許可されているなら
                    if (SpEffect.ignoreDefence == true) power += ms.defence / 4; // 防御力分を上乗せすることで実質無視
                    // クリティカルなら
                    if (SpEffect.critical == true) power *= 2;
                }
            }
            */

            ms?.GetAttack(_enemyStatus.power, transform.position);

            //Instantiate(knifeData.hitEffect, transform.position, Quaternion.identity);
        }
    }

}
