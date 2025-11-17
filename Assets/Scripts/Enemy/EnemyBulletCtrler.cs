using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletCtrler : MonoBehaviour
{
    [SerializeField] float speed = 6f;
    [SerializeField] int damage = 3;
    [SerializeField] float lifeTime = 5f;

    float timeCount = 0;

    public void Initialie()
    {

    }

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
            ms.GetAttack(damage, 0, transform.position);

            Destroy(this.gameObject);
        }
    }
}
