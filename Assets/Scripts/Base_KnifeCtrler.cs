using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_KnifeCtrler : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] int power;

    void Start()
    {
        
    }


    void FixedUpdate()
    {
        transform.Translate(Vector2.up * speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out IDamagable i_d))
        {
            i_d.TakeDamage(power);

            Destroy(this.gameObject);
        }
    }
}
