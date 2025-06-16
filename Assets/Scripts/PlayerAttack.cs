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

    bool a;

    void Start()
    {

    }

    async void AttackTask()
    {
        while (true)
        {
            await UniTask.WaitUntil(() => a == true);

            ShooteBullet();

            await UniTask.Delay((int)(coolTime * 1000));
        }
    }

    GameObject FindEnemy()
    {

        //���͈͓��̓G��z��Ɋi�[
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRadius, targetLayer);

        GameObject nearestObject = null;
        float shortestDistance = Mathf.Infinity; // ������ŏ�����

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
        Instantiate(bullet, this.transform.position, Quaternion.identity);
    }
}
