using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobStatus : MonoBehaviour, IDamagable
{
    [SerializeField] int hp;

    public int hitPoint {  get; private set; }

    void Start()
    {
        hitPoint = hp;
    }

    public void TakeDamage(int a)
    {
        hitPoint -= a;

        if (hitPoint <= 0) Die();
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
