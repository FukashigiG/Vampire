using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class Base_EnemyCtrler : MonoBehaviour
{
    [SerializeField] LayerMask targetLayer;

    Transform target;

    EnemyStatus _enemyStatus;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player").transform;

        _enemyStatus = GetComponent<EnemyStatus>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 dir = (target.position - this.transform.position).normalized;

        transform.Translate(dir * _enemyStatus.moveSpeed * Time.fixedDeltaTime);
    }
}
