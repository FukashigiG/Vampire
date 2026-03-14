using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EP_Bullet : Base_EnemyProps
{
    [SerializeField] GameObject fx_Default;
    [SerializeField] GameObject fx_OnHit;

    ParticleSystem fx;

    float speed = 5f;
    float lifeTime = 5f;

    float timeCount = 0;

    public override void Initialize(int dmg, int elementDmg, GameObject _fx = null)
    {
        base.Initialize(dmg, elementDmg, _fx);

        GameObject fxPrefab;

        if(_fx != null)
        {
            fxPrefab = _fx;
        }
        else
        {
            fxPrefab = fx_Default;
        }

        fx = Instantiate(fxPrefab, this.transform.position, Quaternion.identity, this.transform).GetComponent<ParticleSystem>();
    }

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
            // OnDestroy内でやろうとするとなんかうまくいかない
            fx.transform.parent = null;
            fx.Stop();

            Destroy(this.gameObject);
        }
    }

    // ヒット時の処理
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 当たったものがダメージを受けるものなら
        if (collision.gameObject.TryGetComponent(out IDamagable id))
        {
            id.GetAttack(damage, elementDamage, transform.position);

            Instantiate(fx_OnHit, transform.position, Quaternion.identity);

            // トレイル部分の親子関係の解除
            // 自然に消える演出のための処理
            // OnDestroyないでやろうとするとなんかうまくいかない
            fx.transform.parent = null;
            fx.Stop();

            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        
    }
}
