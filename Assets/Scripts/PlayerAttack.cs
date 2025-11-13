using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UniRx;
using UnityEngine;
using System.Linq;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] KnifeData defKnife;

    [SerializeField] LayerMask targetLayer;

    PlayerStatus status;

    GameObject targetEnemy;

    List<KnifeData_RunTime> hand = new List<KnifeData_RunTime>();

    public Base_P_CharaAbility charaAbility {  get; private set; }



    // アビリティチャージ量
    ReactiveProperty<int> charaAbilityChargeValue = new ReactiveProperty<int>();
    public IReadOnlyReactiveProperty<int> abilityChargeValue => charaAbilityChargeValue;

    // ナイフ初期化直前に発行、秘宝効果で編集できるように
    Subject<(KnifeData_RunTime knifeData, int index)> subject_OnThrowKnife = new Subject<(KnifeData_RunTime, int)>();
    public IObservable<(KnifeData_RunTime knifeData, int index)> onThrowKnife => subject_OnThrowKnife;

    // リロード時に発行、
    Subject<List<KnifeData_RunTime>> subject_OnReload = new();
    public IObservable<List<KnifeData_RunTime>> onReload => subject_OnReload;



    // 攻撃サイクル用トークンソース
    CancellationTokenSource cancellationTokenSource;

    private void Awake()
    {
        status = GetComponent<PlayerStatus>();

        charaAbilityChargeValue.Value = 0;
    }

    void Start()
    {
        StartAttakLoop();
    }

    private void Update()
    {
        targetEnemy = FindEnemy();
    }

    // 攻撃サイクルを開始、停止and再開
    public void StartAttakLoop()
    {
        if(cancellationTokenSource != null)
        {
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
        }

        cancellationTokenSource = new CancellationTokenSource();

        // OnDestroyトークンと元々あるソースのトークンの合成？
        var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(
            cancellationTokenSource.Token,
            this.GetCancellationTokenOnDestroy()
            ).Token;

        AttackTask(linkedToken).Forget();

    }

    async UniTask AttackTask(CancellationToken token)
    {
        try
        {
            while (true)
            {
                // リロード、攻撃の合間にキャンセルされてないか確認

                token.ThrowIfCancellationRequested();

                await Reload(token);

                token.ThrowIfCancellationRequested();

                await ThrowKnives(token);
            }
        }
        catch
        {

        }
        finally
        {

        }
    }

    async UniTask Reload(CancellationToken token)
    {
        // 外部にawaitで利用されているこの関数では、try{}catch{}を使ってはならない（キャンセルが外部に伝わらなくなってしまうため）

        await UniTask.Delay((int)(status.time_ReloadKnives * 500), cancellationToken: token);

        hand = status.inventory.runtimeKnives
                            .OrderBy(x => UnityEngine.Random.value)// 順番をシャッフルして参照（元のリストをいじるわけではない）
                            .Take(status.limit_DrawKnife)// 上から上限まで引く
                            .Select(originalData => new KnifeData_RunTime(originalData))// Selectでオリジナルを元にした新しいインスタンスを
                            .ToList();

        //Debug.Log($"ナイフは{hand.Count}本");

        // 購読先による検知、介入のための発行
        subject_OnReload.OnNext(hand);

        await UniTask.Delay((int)(status.time_ReloadKnives * 500), cancellationToken: token);

        // この処理が2つのdelayで挟まれてるのは、待機時間の真ん中でリロード処理をしたいため
    }

    async UniTask ThrowKnives(CancellationToken token)
    {
        for (int i = 0; i < hand.Count; i++)
        {
            // 攻撃範囲内に敵が現れるまで待つ
            await UniTask.WaitUntil(() => targetEnemy != null, cancellationToken: token);

            // 攻撃対象の方向をVec2型で取得
            Vector2 dir = (targetEnemy.transform.position - this.transform.position).normalized;

            // エディタ上で登録されたナイフデータを取得
            var knife = hand[i];

            // 購読先による介入のための発行
            subject_OnThrowKnife.OnNext((knife, i));

            // ナイフを生成、それをxと置く
            // 編集された可能性のあるKnifeDataで処理を続行
            var x = Instantiate(knife.prefab, this.transform.position, Quaternion.FromToRotation(Vector2.up, dir));

            // ナイフの属性がプレイヤーの得意属性か否か
            bool isElementMatched = status.masteredElements.Contains(knife.element);

            // xを初期化
            x.GetComponent<Base_KnifeCtrler>().Initialize(status.power, knife, status, isElementMatched);

            // ナイフを1本投げるごとにアビリティチャージ
            AbilityCharge();

            // ステータスの持つ数値の分だけ待機
            await UniTask.Delay((int)(status.coolTime_ThrowKnife * 1000), cancellationToken: token);
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

            if (Distance < shortestDistance)
            {
                shortestDistance = Distance;
                nearestObject = hit.gameObject;
            }
        }

        return nearestObject;
    }

    // 外部からキャラアビリティをセットするための関数
    public void SetCharaAbility(Base_P_CharaAbility ability)
    {
        // 既に割り当て済みならReturn
        if(charaAbility != null) return;

        // 渡されたものの新規インスタンスを生成、それを代入
        charaAbility = UnityEngine.Object.Instantiate(ability);

        // 初期化
        charaAbility.Initialize(status);


    }

    // アビリティチャージ
    public void AbilityCharge(int value = 1)
    {
        // アビリティが無いならリターン
        if (charaAbility == null) return;

        charaAbilityChargeValue.Value += value;
    }

    // アビリティの実行
    public void ExecuteCharaAbility()
    {
        // アビリティがないならリターン
        if (charaAbility == null) return;

        // 必要なチャージ量に届いてなければリターン
        if (charaAbilityChargeValue.Value < charaAbility.requireChargeValue) return;

        // アビリティチャージ量をリセット
        charaAbilityChargeValue.Value = 0;

        // charaAbility内の関数を実行
        charaAbility.ActivateAbility();
    }

    private void OnDestroy()
    {
        if(cancellationTokenSource != null)
        {
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
            cancellationTokenSource = null;
        }

        subject_OnThrowKnife.Dispose();

        subject_OnReload.Dispose();
    }
}
