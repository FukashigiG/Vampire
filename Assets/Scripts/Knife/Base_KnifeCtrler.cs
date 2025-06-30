using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_KnifeCtrler : MonoBehaviour
{
    [SerializeField] KnifeData knifeData;

    protected float speed;
    protected float lifeTime;
    protected int power;

    protected virtual void Start()
    {
        power = knifeData.power;

        lifeTime = 1;
    }

    //初期化用メゾット
    public void Initialize(float s)
    {
        speed = s;
    }

    protected virtual void FixedUpdate()
    {
        // 進む
        transform.Translate(Vector2.up * speed * Time.fixedDeltaTime);

        // 寿命
        lifeTime -= Time.fixedDeltaTime;
        if(lifeTime <= 0 ) Destroy(this.gameObject);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        // もし当たったものがダメージを受けるものだったらダメージを与える
        if(collision.TryGetComponent(out Base_MobStatus ms))
        {
            bool shouldDestroyThis = true;

            foreach(var SpEffect in knifeData.specialEffects)
            {
                if(SpEffect != null)
                {
                    SpEffect.OnHitSpecialEffect(ms);

                    if(SpEffect.DestroyBullet == false) shouldDestroyThis = false;
                    if (SpEffect.IgnoreDefence == true) power += ms.defence;
                }
            }

            ms.TakeDamage(power, transform.position);

            if (shouldDestroyThis) Destroy(this.gameObject);
        }
    }
}
