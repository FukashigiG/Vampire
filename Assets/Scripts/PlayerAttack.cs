using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] KnifeData defKnife;

    [SerializeField] float coolTime_ThrowKnife;
    [SerializeField] float time_ReloadKnives;

    [SerializeField] LayerMask targetLayer;

    PlayerStatus status;

    GameObject targetEnemy;

    public List<KnifeData> availableKnifes { get; private set; } = new List<KnifeData>();

    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
    CancellationToken _token;

    void Start()
    {
        _token = cancellationTokenSource.Token;

        status = GetComponent<PlayerStatus>();

        availableKnifes.Add(defKnife);

        AttackTask(_token).Forget();
    }

    private void Update()
    {
        targetEnemy = FindEnemy();
    }

    public void AddKnife(KnifeData x)
    {
        availableKnifes.Add(x);
    }

    async UniTask AttackTask(CancellationToken token)
    {
        while (true)
        {
            await UniTask.Delay((int)(time_ReloadKnives * 1000));

            await ThrowKnives(_token);
        }
    }

    // UŒ‚‘ÎÛ‚Ì’Tõ
    GameObject FindEnemy()
    {

        //ˆê’è”ÍˆÍ“à‚Ì“G‚ğ”z—ñ‚ÉŠi”[
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, status.eyeSight, targetLayer);

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

    async UniTask ThrowKnives(CancellationToken token)
    {
        for (int i = 0; i < availableKnifes.Count; i++)
        {
            await UniTask.WaitUntil(() => targetEnemy != null, cancellationToken: token);

            Vector2 dir = (targetEnemy.transform.position - this.transform.position).normalized;

            // ƒiƒCƒt‚ğ¶¬A‚»‚ê‚ğx‚Æ’u‚­
            var x = Instantiate(availableKnifes[i].prefab, this.transform.position, Quaternion.FromToRotation(Vector2.up, dir));
            // x‚ğ‰Šú‰»
            x.GetComponent<Base_KnifeCtrler>().Initialize(status.throwPower);

            await UniTask.Delay((int)(coolTime_ThrowKnife * 1000), cancellationToken: token);
        }
    }

    private void OnDestroy()
    {
        cancellationTokenSource.Cancel();
        cancellationTokenSource.Dispose();
    }
}
