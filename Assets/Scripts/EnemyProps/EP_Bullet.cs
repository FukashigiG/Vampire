using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EP_Bullet : Base_EnemyProps
{
    [SerializeField] GameObject fx_OnHit;
    [SerializeField] ParticleSystem fx_Trail;

    float speed = 5f;
    float lifeTime = 5f;

    float timeCount = 0;

    void Update()
    {
        // 移動
        transform.Translate(Vector2.up * speed * Time.deltaTime);

        // 寿命計算
        timeCount += Time.deltaTime;
        if (timeCount >= lifeTime)
        {
            // トレイル部分の親子関係の解除
            // 自然に消える演出のための処理
            // OnDestroyないでやろうとするとなんかうまくいかない
            fx_Trail.transform.parent = null;
            fx_Trail.Stop();

            Destroy(this.gameObject);
        }
    }

    // ヒット時の処理
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 当たったものがダメージを受けるものなら
        if (collision.gameObject.TryGetComponent(out Base_MobStatus ms))
        {
            ms.GetAttack(damage, elementDamage, transform.position);

            Instantiate(fx_OnHit, transform.position, Quaternion.identity);

            // トレイル部分の親子関係の解除
            // 自然に消える演出のための処理
            // OnDestroyないでやろうとするとなんかうまくいかない
            fx_Trail.transform.parent = null;
            fx_Trail.Stop();

            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        
    }
}
