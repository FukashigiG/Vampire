using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EP_Bullet : Base_EnemyProps
{
    float speed = 8f;
    float lifeTime = 5f;

    float timeCount = 0;

    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);

        timeCount += Time.deltaTime;

        if (timeCount >= lifeTime ) Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Base_MobStatus ms))
        {
            ms.GetAttack(damage, elementDamage, transform.position);

            Destroy(this.gameObject);
        }
    }
}
