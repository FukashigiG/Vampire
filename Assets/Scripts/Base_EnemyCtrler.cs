using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class Base_EnemyCtrler : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    [SerializeField] LayerMask targetLayer;

    Transform target;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 dir = (target.position - this.transform.position).normalized;

        transform.Translate(dir * moveSpeed * Time.fixedDeltaTime);
    }
}
