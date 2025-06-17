using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] GameObject bullet;

    [SerializeField] float detectionRadius;
    [SerializeField] float coolTime;

    [SerializeField] LayerMask targetLayer;

    GameObject targetEnemy;

    void Start()
    {
        AttackTask();
    }

    private void Update()
    {
        targetEnemy = FindEnemy();
    }

    async void AttackTask()
    {
        while (true)
        {
            await UniTask.WaitUntil(() => targetEnemy != null);

            ShooteBullet();

            await UniTask.Delay((int)(coolTime * 1000));
        }
    }

    GameObject FindEnemy()
    {

        //ˆê’è”ÍˆÍ“à‚Ì“G‚ğ”z—ñ‚ÉŠi”[
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRadius, targetLayer);

        GameObject nearestObject = null;
        float shortestDistance = Mathf.Infinity; // –³ŒÀ‘å‚Å‰Šú‰»

        foreach (Collider2D hit in hits)
        {
            float Distance = Vector2.Distance(transform.position, hit.transform.position);

            if(Distance < shortestDistance )
            {
                shortestDistance = Distance;
                nearestObject = hit.gameObject;
            }
        }

        return nearestObject;
    }

    void ShooteBullet()
    {
        Vector2 dir = (targetEnemy.transform.position - this.transform.position);

        Instantiate(bullet, this.transform.position, Quaternion.FromToRotation(Vector2.up, dir));
    }
}
