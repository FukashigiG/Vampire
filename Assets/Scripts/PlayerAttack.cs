using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] GameObject bullet;

    [SerializeField] float coolTime_ThrowKnife;
    [SerializeField] float time_ReloadKnives;

    [SerializeField] LayerMask targetLayer;

    PlayerStatus status;

    GameObject targetEnemy;

    List<GameObject> availableKnifes = new List<GameObject>();

    void Start()
    {
        status = GetComponent<PlayerStatus>();

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

        //一定範囲内の敵を配列に格納
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, status.eyeSight, targetLayer);

        GameObject nearestObject = null;
        float shortestDistance = Mathf.Infinity; // 無限大で初期化

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

            // ナイフを生成、それをxと置く
            var x = Instantiate(availableKnifes[i], this.transform.position, Quaternion.FromToRotation(Vector2.up, dir));
            // xを初期化
            x.GetComponent<Base_KnifeCtrler>().Initialize(status.throwPower);

            await UniTask.Delay((int)(coolTime_ThrowKnife * 1000));
        }
    }
}
