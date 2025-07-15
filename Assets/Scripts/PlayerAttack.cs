using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UniRx;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] KnifeData defKnife;

    [SerializeField] float coolTime_ThrowKnife;
    [SerializeField] float time_ReloadKnives;

    [SerializeField] LayerMask targetLayer;

    PlayerStatus status;

    GameObject targetEnemy;

    List<KnifeData> availableKnifes = new List<KnifeData>();

    // ナイフ生成直前に発行、秘宝効果で編集できるように
    public Subject<KnifeData> onThrowKnife { get; private set; } = new Subject<KnifeData>();

    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
    CancellationToken _token;

    private void Awake()
    {
        status = GetComponent<PlayerStatus>();

        _token = cancellationTokenSource.Token;
    }

    void Start()
    {
        AttackTask(_token).Forget();
    }

    private void Update()
    {
        targetEnemy = FindEnemy();
    }

    // 攻撃サイクル処理
    async UniTask AttackTask(CancellationToken token)
    {
        while (true)
        {
            await Reload();

            await ThrowKnives(_token);
        }
    }

    // 攻撃対象の探索
    GameObject FindEnemy()
    {

        //一定範囲内の敵を配列に格納
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, status.eyeSight, targetLayer);

        GameObject nearestObject = null;
        float shortestDistance = Mathf.Infinity; // 無限大で初期化

        // 一番近い敵を探索
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

    async UniTask Reload()
    {
        availableKnifes = status.inventory.runtimeKnives;

        await UniTask.Delay((int)(time_ReloadKnives * 1000));

        Debug.Log("reload : " + availableKnifes.Count);
    }

    async UniTask ThrowKnives(CancellationToken token)
    {
        for (int i = 0; i < availableKnifes.Count; i++)
        {
            // 攻撃範囲内に敵が現れるまで待つ
            await UniTask.WaitUntil(() => targetEnemy != null, cancellationToken: token);

            // 攻撃対象の方向をVec2型で取得
            Vector2 dir = (targetEnemy.transform.position - this.transform.position).normalized;

            // エディタ上で登録されたScriptableObjectを取得
            var originalKnifeData = availableKnifes[i];

            // ScriptableObjectをInstantiateして、実行時専用のコピーを生成する
            // こうすることで、実際のScriptableObjectの内容を書き換えることなく、編集されたKnifeDataを扱うことができる
            var knife = Instantiate(originalKnifeData);

            // 購読先による介入のための発行
            onThrowKnife.OnNext(knife);

            // ナイフを生成、それをxと置く
            // 編集された可能性のあるKnifeDataで処理を続行
            var x = Instantiate(knife.prefab, this.transform.position, Quaternion.FromToRotation(Vector2.up, dir));

            // xを初期化
            x.GetComponent<Base_KnifeCtrler>().Initialize(status.throwPower, knife);

            await UniTask.Delay((int)(coolTime_ThrowKnife * 1000), cancellationToken: token);
        }
    }

    private void OnDestroy()
    {
        cancellationTokenSource.Cancel();
        cancellationTokenSource.Dispose();

        onThrowKnife.Dispose();
    }
}
