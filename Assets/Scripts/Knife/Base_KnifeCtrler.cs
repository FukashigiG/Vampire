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
        // もし当たったもんがダメージを受けるものだったらダメージ
        if(collision.TryGetComponent(out IDamagable i_d))
        {
            i_d.TakeDamage(power);

            Destroy(this.gameObject);
        }
    }
}
