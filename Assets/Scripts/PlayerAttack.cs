using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] GameObject bullet;

    [SerializeField] float detectionRadius;
    [SerializeField] float coolTime_ThrowKnife;
    [SerializeField] float time_ReloadKnives;

    [SerializeField] LayerMask targetLayer;

    GameObject targetEnemy;

    List<GameObject> availableKnifes = new List<GameObject>();

    void Start()
    {
        availableKnifes.Add(bullet);
        availableKnifes.Add(bullet);

        AttackTask();
    }

    private void Update()
    {
        targetEnemy = FindEnemy();
    }

    public void AddKnife(KnifeData x)
    {
        availableKnifes.Add(x.prefab);
    }

    async void AttackTask()
    {
        while (true)
        {
            await UniTask.Delay((int)(time_ReloadKnives * 1000));

            await ThrowKnives();
        }
    }

    GameObject FindEnemy()
    {

        //àÍíËîÕàÕì‡ÇÃìGÇîzóÒÇ…äiî[
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRadius, targetLayer);

        GameObject nearestObject = null;
        float shortestDistance = Mathf.Infinity; // ñ≥å¿ëÂÇ≈èâä˙âª

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

    async UniTask ThrowKnives()
    {
        for (int i = 0; i < availableKnifes.Count; i++)
        {
            await UniTask.WaitUntil(() => targetEnemy != null);

            Vector2 dir = (targetEnemy.transform.position - this.transform.position);

            Instantiate(availableKnifes[i], this.transform.position, Quaternion.FromToRotation(Vector2.up, dir));

            await UniTask.Delay((int)(coolTime_ThrowKnife * 1000));
        }
    }
}
