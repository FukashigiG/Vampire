using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] GameObject bullet;

    [SerializeField] Collider2D ATcollider;

    void Start()
    {
    }

    void ShooteBullet()
    {
        Instantiate(bullet, this.transform.position, Quaternion.identity);
    }
}
