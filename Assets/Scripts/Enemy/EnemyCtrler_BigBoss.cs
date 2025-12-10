using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UniRx;
using System.Linq;
using System;
using Random = UnityEngine.Random;

public class EnemyCtrler_BigBoss : Base_EnemyCtrler
{
    class BossAction
    {
        public EnemyData.BossActionData data {  get; private set; }

        // 内部計算用
        [HideInInspector] public int currentWeight { get; private set; }

        public bool isExecuted { get; set; } = false;

        public BossAction(EnemyData.BossActionData _data)
        {
            data = _data;

            Initialize_CullentWeight();
        }

        // 重みを初期化
        public void Initialize_CullentWeight()
        {
            currentWeight = data.baseWeight;
        }

        // 現在の重みを減衰率で減らす
        public void DeceyCullentWeight()
        {
            currentWeight = (int)(currentWeight * data.decayRate);
        }
    }

    static Subject<Base_BossEnemyAct> subject_OnAct = new Subject<Base_BossEnemyAct>();
    public static IObservable<Base_BossEnemyAct> onAct => subject_OnAct;

    // ボスの行動
    List<BossAction> actions = new List<BossAction>();

    bool attackable = true;

    // 行動回数：行動が何回目か
    int cullentActCount = 0;

    CancellationToken token;

    protected override void Awake()
    {
        base.Awake();

        token = gameObject.GetCancellationTokenOnDestroy();
    }

    protected override void Start()
    {
        base.Start();

        // それぞれのactionの重みを初期化
        foreach (var actData in _enemyStatus._enemyData.bossActions)
        {
            actions.Add(new BossAction(actData));
        }

        LifeCycle().Forget();
    }

    protected override void FixedUpdate()
    {
    }

    protected override void HandleAI()
    {
    }

    async UniTask LifeCycle()
    {
        try
        {
            while (true)
            {
                cullentActCount++;

                Base_BossEnemyAct act = DecideNextAction();

                subject_OnAct.OnNext(act);

                await act.Action(this, token);

                Debug.Log("oppo");

                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: token);
            }
        }
        catch
        {
            Debug.Log("wwwww");
        }
    }

    Base_BossEnemyAct DecideNextAction()
    {
        // N回目の行動として設定されてるやつを探す
        var XXX = actions.Find(x => x.data.triggerType == EnemyData.BossActionData.TriggerType.SpecifiedActCount && x.data.targetActCount == cullentActCount);
        // あったらそれのact部分を返して終わり
        if (XXX != null)
        {
            var act = XXX.data.actionLogic;

            // 発動が1回制限タイプなら、リストから削除
            // 普通は設定されてる想定
            // というかどっちみち1回しか発動しなくね？？？
            // やっぱ制限とか無しに削除していいわ
            /*if (XXX.data.isOneTimeOnry) */actions.Remove(XXX);

            return XXX.data.actionLogic;
        }

        // HPがN%を下回ったときの行動として設定されていて、条件を満たしてるやつを探す
        var YYY = actions.Find(x => x.data.triggerType == EnemyData.BossActionData.TriggerType.HpThreshold && (_enemyStatus.hitPoint.Value / _enemyStatus.maxHP) * 100 <= x.data.thresholdHpRate_Percent);
        // あったらそれのact部分を返して終わり
        if (YYY != null)
        {
            var act = YYY.data.actionLogic;

            // リストから削除
            // 削除しないと無限に発動してしまうため
            actions.Remove(YYY);

            return YYY.data.actionLogic;
        }

        Debug.Log(actions.Count);

        // プレイヤーとの距離を取得
        float distance = (target.position - this.transform.position).magnitude;

        // 通常抽選タイプのうち、適正範囲の行動を取得
        List<BossAction> collectRangeActions = actions
            .Where(x => x.data.triggerType == EnemyData.BossActionData.TriggerType.WeightRandom)
            .Where(x => x.data.actionLogic.range_Min <= distance && x.data.actionLogic.range_Max >= distance)
            .ToList();

        Dictionary<BossAction, int> weightDictionaly = new();
        int totalWeight = 0;

        // 重みと紐づける登録
        foreach (BossAction act in collectRangeActions)
        {
            // 最長距離が短い技ほど発動確率を上げるための処理
            int finalyWeight;

            if (act.data.actionLogic.range_Max < 10f)
            {
                finalyWeight = (int)(act.currentWeight * (10f / act.data.actionLogic.range_Max));
            }
            else
            {
                finalyWeight = act.currentWeight;
            }

            // 重みが１未満にならないように調整
            if (finalyWeight <= 0) finalyWeight = 1;

            // 最終的な重みを辞書で紐づけ
            weightDictionaly.Add(act, finalyWeight);
            // 
            totalWeight += finalyWeight;
        }

        int randomPoint = Random.Range(1, totalWeight + 1);

        float currentSum = 0f;
        BossAction selectedAction = null;

        // 重み抽選
        foreach (var pair in weightDictionaly)
        {
            currentSum += pair.Value;
            if (randomPoint <= currentSum)
            {
                selectedAction = pair.Key;
                break;
            }
        }

        // 選ばれた技は重みを減らす（連続使用確率を下げる）
        selectedAction.DeceyCullentWeight();

        // 選ばれなかった行動の重みを初期化
        foreach (var act in actions)
        {
            if(act != selectedAction)
            {
                act.Initialize_CullentWeight();
            }
        }

        return selectedAction.data.actionLogic;
    }
}
