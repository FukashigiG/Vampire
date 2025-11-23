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
    [System.Serializable]
    class BossAction
    {
        [field: SerializeField] public Base_BossEnemyAct actionLogic { get; private set; } // 処理部分
        [SerializeField, Range(0, 100)] int baseWeight = 50; // 基礎確率の重み
        [SerializeField, Range(0.1f, 0.9f)] float decayRate = 0.5f; // 連続使用時の重み減衰率

        // 内部計算用
        [HideInInspector] public int currentWeight { get; private set; }

        // 重みを初期化
        public void Initialize_CullentWeight()
        {
            currentWeight = baseWeight;
        }

        // 現在の重みを減衰率で減らす
        public void DeceyCullentWeight()
        {
            currentWeight = (int)(currentWeight * decayRate);
        }
    }

    static Subject<Base_BossEnemyAct> subject_OnAct = new Subject<Base_BossEnemyAct>();
    public static IObservable<Base_BossEnemyAct> onAct => subject_OnAct;

    // ボスの行動

    [SerializeField] List<BossAction> actions;

    bool attackable = true;

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
        foreach (var act in actions)
        {
            act.Initialize_CullentWeight();
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
        while (true)
        {
            Base_BossEnemyAct act = DecideNextAction();

            subject_OnAct.OnNext(act);

            await act.Action(this);

            await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: token);
        }
    }

    Base_BossEnemyAct DecideNextAction()
    {
        float distance = (target.position - this.transform.position).magnitude;

        // 適正範囲の行動を取得
        List<BossAction> collectRangeActions = actions.Where(x => x.actionLogic.range_Min <= distance && x.actionLogic.range_Max >= distance).ToList();

        Dictionary<BossAction, int> weightDictionaly = new();
        int totalWeight = 0;

        // 重みと紐づける登録
        foreach (BossAction act in collectRangeActions)
        {
            // 最長距離が短い技ほど発動確率を上げるための処理
            int finalyWeight;

            if (act.actionLogic.range_Max < 10f)
            {
                finalyWeight = (int)(act.currentWeight * (10f / act.actionLogic.range_Max));
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

        return selectedAction.actionLogic;
    }
}
