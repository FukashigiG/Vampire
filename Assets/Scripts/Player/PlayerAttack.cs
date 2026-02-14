using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UniRx;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using Unity.Cinemachine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] LayerMask targetLayer;

    [SerializeField] GameObject closerCam;

    [SerializeField] GameObject fx_Throw;

    [SerializeField] GameObject CutInUI;

    PlayerStatus status;

    public GameObject targetEnemy {  get; private set; }

    // いわゆる手札
    ReactiveCollection<KnifeData_RunTime> hand = new ReactiveCollection<KnifeData_RunTime>();
    public IReadOnlyReactiveCollection<KnifeData_RunTime> hand_RC => hand;

    public Base_P_CharaAbility charaAbility {  get; private set; }

    // アビリティチャージ量
    ReactiveProperty<int> charaAbilityChargeValue = new ReactiveProperty<int>(0);
    // ↑の参照部分公開用
    public IReadOnlyReactiveProperty<int> abilityChargeValue => charaAbilityChargeValue;

    // ナイフ初期化直前に発行、秘宝効果で編集できるように
    Subject<KnifeData_RunTime> subject_OnThrowKnife = new Subject<KnifeData_RunTime>();
    public IObservable<KnifeData_RunTime> onThrowKnife => subject_OnThrowKnife;

    // リロード時に発行、
    Subject<ReactiveCollection<KnifeData_RunTime>> subject_OnReload = new();
    public IObservable<ReactiveCollection<KnifeData_RunTime>> onReload => subject_OnReload;

    // 攻撃サイクル用トークンソース
    CancellationTokenSource cancellationTokenSource;

    public void Initialize(PlayerStatus _status, Base_P_CharaAbility ability)
    {
        status = _status;

        status.onDie.Subscribe(x =>
        {
            if(cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
                cancellationTokenSource.Dispose();

                cancellationTokenSource = null;
            }

        }).AddTo(this);

        SetCharaAbility(ability);

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

                await ThrowAllHands(token);
            }
        }
        catch (OperationCanceledException)
        {
            // キャンセル時は正常終了なので何もしない
        }
        catch (System.Exception e)
        {
            // 【重要】エラー原因を特定するためにログを出す
            Debug.LogException(e);

            // エラーでループが止まると困る場合は、ここで再帰呼び出しや復帰処理が必要ですが、
            // まずはログを出して原因を突き止めるのが先決です。
        }
    }

    async UniTask Reload(CancellationToken token)
    {
        // 外部にawaitで利用されているこの関数では、try{}catch{}を使ってはならない（キャンセルが外部に伝わらなくなってしまうため）

        // まず手持ちを空にする
        hand.Clear();

        await UniTask.Delay((int)(status.time_ReloadKnives * 1000 / 2), cancellationToken: token);

        List<KnifeData_RunTime> drawnKnives = status.inventory.runtimeKnives
                            .OrderBy(x => UnityEngine.Random.value)// 順番をシャッフルして参照（元のリストをいじるわけではない）
                            .Take(status.limit_DrawKnife)// 上から上限まで
                            .ToList();

        SetHand(drawnKnives);

        //Debug.Log($"ナイフは{hand.Count}本");

        // 購読先による検知、介入のための発行
        subject_OnReload.OnNext(hand);

        await UniTask.Delay((int)(status.time_ReloadKnives * 1000 / 2), cancellationToken: token);
    }

    async UniTask ThrowAllHands(CancellationToken token)
    {
        while(hand.Count > 0)
        {
            // 攻撃範囲内に敵が現れる,かつ行動可能になるまで待つ
            await UniTask.WaitUntil(() => targetEnemy != null && status.actable, cancellationToken: token);

            // 何かしらの理由で手札が無かったらループを抜けてリロードへ
            if (hand.Count == 0)
            {
                break; 
            }

            // handの先頭を取得
            var knife = hand[0];

            // handの先頭を削除 早めに済ませておく
            hand.RemoveAt(0);

            // 攻撃対象の方向をVec2型で取得
            Vector2 dir = (targetEnemy.transform.position - this.transform.position).normalized;

            // それをQuaternionに変換
            Quaternion baseRotation = Quaternion.FromToRotation(Vector2.up, dir);

            ThrowKnife(knife, baseRotation);

            Instantiate(fx_Throw, transform.position + (Vector3)dir, baseRotation);

            // ステータスの持つ数値の分だけ待機
            await UniTask.Delay((int)(status.coolTime_ThrowKnife * 1000), cancellationToken: token);
        }
    }

    public void ThrowKnife(KnifeData_RunTime knife, Quaternion baseRotation)
    {
        // 購読先による介入のための発行
        subject_OnThrowKnife.OnNext(knife);

        int count_multiKnife = knife.count_Multiple;

        // ナイフ重複度だけオブジェクトを生成
        for (int i = 0; i < count_multiKnife; i++)
        {
            float angleOffset = 0;

            // 重複カウントが２以上なら以下の計算を実行
            if (count_multiKnife > 1)
            {
                // 今投げる角度を求める
                angleOffset = Mathf.Lerp(-10 / 2f, 10 / 2f, (float)i / (count_multiKnife - 1));
            }

            // Quaternionに変換
            Quaternion rotationOffset = Quaternion.Euler(0, 0, angleOffset);

            // ベースの方向と合成
            Quaternion finalRotation = baseRotation * rotationOffset;

            // ナイフを生成、それをxと置く
            // 編集された可能性のあるKnifeDataで処理を続行
            var x = Instantiate(knife.prefab, this.transform.position, finalRotation);

            // ナイフの属性がプレイヤーの得意属性か否か
            bool isElementMatched = status.masteredElements.Contains(knife.element);

            // xを初期化
            // この文以降でこのxを参照してはならない（Initialize）
            x.GetComponent<Base_KnifeCtrler>().Initialize(status.power, knife, status, isElementMatched);
        }

        // ナイフを1回投げるごとにアビリティチャージ
        AbilityCharge();
    }

    // リストまたは単体のナイフを手持ちに加える
    public void SetHand(List<KnifeData_RunTime> list = null, KnifeData_RunTime knife = null)
    {
        if (list != null)
        {
            // Selectでオリジナルを元にした新しいインスタンスを
            // inventry内のオリジナルデータを渡されることを想定している
            // AddRangeを用いることでそれら一つ一つが通知される
            hand.AddRange(list.Select(originalData => new KnifeData_RunTime(originalData)));
        }

        if (knife != null)
        {
            hand.Add(new KnifeData_RunTime(knife));
        }
    }

    // N番目のナイフを手持ちから捨てる
    public void TrashKnife(int index)
    {
        hand.RemoveAt(index);
    }

    public int GetHandCount()
    {
        return hand.Count;
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

    // キャラアビリティをセットするための関数
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
    public async UniTask ExecuteCharaAbility()
    {
        // アビリティがないならリターン
        if (charaAbility == null) return;

        // 必要なチャージ量に届いてなければリターン
        if (charaAbilityChargeValue.Value < charaAbility.requireChargeValue) return;

        // 行動可能状態でないならリターン
        if(! status.actable) return;

        // アビリティチャージ量をリセット
        charaAbilityChargeValue.Value = 0;

        // アビリティ中はダメージを受けず、ノックバックせず、他の行動を許可しない
        status.count_Actable++;
        status.count_PermissionDamage++;
        status.count_PermissionKnickBack++;

        var token = this.GetCancellationTokenOnDestroy();

        GameObject cutinUI = Instantiate(CutInUI, Vector2.zero, Quaternion.identity, GameObject.Find("Canvas").transform);
        cutinUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(266, 0);

        GameAdmin.Instance.SetTimeScaleValue(0.5f);

        closerCam.SetActive(true);

        await UniTask.Delay((int)(1000 * Time.timeScale), cancellationToken: token);

        GameAdmin.Instance.SetTimeScaleValue(1f);

        closerCam.SetActive(false);


        try
        {

            // charaAbility内の関数を実行
            await charaAbility.ActivateAbility(token);
        }
        finally
        {
            status.count_Actable--;
            status.count_PermissionDamage--;
            status.count_PermissionKnickBack--;
        }
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
