using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine.UIElements;

public class Base_MobStatus : MonoBehaviour, IDamagable
{
    [SerializeField] GameObject damageTxt;
    protected Color color_DamageTxt;

    public int maxHP { get; protected set; }

    protected ReactiveProperty<int> _hitPoint = new ReactiveProperty<int>();
    public ReadOnlyReactiveProperty<int> hitPoint => _hitPoint.ToReadOnlyReactiveProperty();

    // 基礎ステータス値
    protected int base_Defence;
    protected int base_Power;
    protected int base_MoveSpeed;

    // バフ量
    public int enhancementRate_Defence = 0;
    public int enhancementRate_Power = 0;
    public int enhancementRate_MoveSpeed = 0;

    // 基礎ステータスとバフを計算した結果を返すプロパティ
    public int defence { get { return (int)(base_Defence * (1f + (enhancementRate_Defence / 100f))); } }
    public int power { get { return (int)(base_Power * (1f + (enhancementRate_Power / 100f))); } }
    public int moveSpeed { get { return (int)(base_MoveSpeed * (1f + (enhancementRate_MoveSpeed / 100f))); } }

    // 特殊状態
    // これらの変数はステータスエフェクトからのみ書き換えられなければならない
    public bool actable = true;// { get; protected set; }
    public bool isArrowDamage = true;// {  get; protected set; }
    public bool isArrowHit = true;// {  get; protected set; }
    public bool damageOverTime = false;
    public bool onRegeneration = false;

    float ratio_SlipDamage = 2;
    float ratio_Regene = 4;

    public int applied_AllowKnickBack = 0;
    bool allowKnickBack => applied_AllowKnickBack == 0;

    protected Subject<(Vector2 position, int amount)> subject_OnDamaged = new Subject<(Vector2, int)>();
    protected Subject<Unit> subject_OnSecond = new Subject<Unit>();
    protected static Subject<(Base_MobStatus status, Base_StatusEffectData effect, float duration, int amount)> subject_OnGetStatusEffect = new();
    protected static Subject<(Base_MobStatus status , int value)> subject_OnDie = new Subject<(Base_MobStatus, int)>();
    /*static にすることで、どの Enemy インスタンスからでもこのSubjectにアクセスし、
     * イベントを発行できるようになる
     * また、プレイヤー側で単一のSubjectを購読するだけで、
     * 全ての敵の撃破イベントをキャッチできる*/

    // 同一効果（xxナイフのバフ、yy秘宝の火炎）などの重複を防ぐための管理
    Dictionary<string, CancellationTokenSource> activeStatusEffects = new();
    // 同種の効果（何かしらがトリガーの火炎、凍結など）を管理
    public ReactiveDictionary<Base_StatusEffectData, int> activeStatusTypeCounts = new();

    // 被ダメージ数値を書き換える際に使用されるフィルター
    // バグ防止のためにもフィルターは複数使えるべきではないのでは？
    public List<System.Func<int, int>> damageFilters {  get; private set; } = new();

    SpriteRenderer _renderer;
    Collider2D _collider;

    float timeCount = 0f;

    protected virtual void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
    }

    protected virtual void Start()
    {
        
    }

    void Update()
    {
        timeCount += Time.deltaTime;

        if(timeCount > 1f)
        {
            timeCount = 0f;

            SecondUpdate();
        }
    }

    // 毎秒処理
    void SecondUpdate()
    {
        // スリップダメージを受ける設定なら、
        if (damageOverTime) TakeDamage((int)((float)maxHP / 100f * ratio_SlipDamage));
        // リジェネを受ける設定なら、
        if (onRegeneration) HealHP((int)((float)maxHP / 100f * ratio_Regene));

        // 毎秒の通知
        subject_OnSecond.OnNext(Unit.Default);
    }

    // 状態変化効果を適用する統合メソッド
    public void ApplyStatusEffect(Base_StatusEffectData effect, string effectID, float duration, int amount = 0)
    {
        // すでに同じ効果がかかっている場合は、一度キャンセルしてから上書きする
        if (activeStatusEffects.ContainsKey(effectID))
        {
            activeStatusEffects[effectID].Cancel();
            activeStatusEffects[effectID].Dispose();
            //activeStatusEffects.Remove(type);
        }

        // 新しいトークンソースを用意
        var cts = new CancellationTokenSource();

        activeStatusEffects[effectID] = cts;

        // イベント発行
        // 変数一式を渡すので、数値を購読先が編集できる
        // （例：特定の状態効果の時間を延長する秘宝）
        subject_OnGetStatusEffect.OnNext((this, effect, duration, amount));

        // タスクの実行
        StatusEffectTask(effect, effectID, duration, amount, cts).Forget();
    }

    // 状態変化効果の非同期処理
    async UniTask StatusEffectTask(Base_StatusEffectData effect, string effectID, float duration, int amount, CancellationTokenSource cts)
    {
        // カウントの追加
        if(! activeStatusTypeCounts.ContainsKey(effect)) activeStatusTypeCounts[effect] = 0;
        activeStatusTypeCounts[effect]++;

        // 渡されたエフェクトの適用時効果を実行
        effect.Apply(this, amount);

        try
        {
            if(effect.IsTickingEffect)
            {
                await effect.Tick(this, duration, amount, cts.Token);
            }
            else
            {
                // 通常の待ち処理
                await UniTask.Delay((int)(duration * 1000), cancellationToken: cts.Token);
            }
        }
        catch (System.OperationCanceledException)
        {
            Debug.Log($"{effect.effectName}.effect was cancelled");
        }
        finally
        {
            if (activeStatusTypeCounts.ContainsKey(effect))
            {
                // カウントを減らす
                activeStatusTypeCounts[effect]--;

                // カウントが0なら削除
                if (activeStatusTypeCounts[effect] <= 0) activeStatusTypeCounts.Remove(effect);
            }


            effect.Remove(this, amount);

            // Dictionaryに自分に宛てたトークンソースが残っているのであれば、それを削除
            if (activeStatusEffects.ContainsKey(effectID) && activeStatusEffects[effectID] == cts)
            {
                activeStatusEffects.Remove(effectID);
            }

            cts.Dispose();
        }

    }

    // 指定した種類の状態異常がアクティブかを返す関数
    public bool IsStatusEffectTypeActive(Base_StatusEffectData effect)
    {
        return activeStatusTypeCounts.ContainsKey(effect) && activeStatusTypeCounts[effect] > 0;
    }


    // 攻撃を受ける処理
    public virtual bool GetAttack(int damagePoint, int elementPoint, Vector2 damagedPosi, bool isCritical = false, bool isIgnoreDefence = false)
    {
        //被撃許容状態でないなら、falseを返して処理を終了する
        if(! isArrowHit) return false;

        int K = 50;

        // クリティカルなら基礎ダメージ量を2倍
        if(isCritical) damagePoint *= 2;

        // 防御無視でないなら、防御計算
        if (!isIgnoreDefence) damagePoint = (int)(damagePoint / ((float)(K + defence) / K));
        // damagePointを0以下にしない
        if (damagePoint < 0) damagePoint = 0;

        // ダメージ計算式
        int damage = damagePoint + elementPoint;

        // 0以下にならないように
        if (damage <= 0) damage = 1;

        TakeDamage(damage);

        KnockBack(damagedPosi, 1);

        return true;
    }

    // ダメージ処理
    public virtual int TakeDamage(int value)
    {
        if (!isArrowDamage) return 0;

        // フィルターにダメージ数値を通す（量がかわる）
        foreach(var filter in damageFilters)
        {
            value = filter(value);
        }

        if (value <= 0) return 0;

        // ダメージを受けることを通知
        // これにより、ここで扱うvalueの値は変わらない
        // （値のコピーが渡される）
        subject_OnDamaged.OnNext((transform.position, value));

        // hpからvalue分マイナスする
        _hitPoint.Value -= value;

        //ダメージテキストを出す処理
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);

        var x = Instantiate(damageTxt, screenPoint, Quaternion.identity, GameObject.Find("Parent_DamageTxt").transform);

        x.GetComponent<DamageTxtCtrler>().Initialize(value, color_DamageTxt);


        if (_hitPoint.Value <= 0)
        {
            // hpがないなら死亡判定処理
            Die();
        }
        else
        {
            // 生きてるなら演出
            damageFlash().Forget();
        }

        return value;
    }

    /*
     *上二つの関数が分かれてるのは、防御計算をしたい場合としたくない場合に両対応するため 
     */

    async UniTask damageFlash()
    {
        var token = this.GetCancellationTokenOnDestroy();

        _renderer.color = Color.red;

        await UniTask.Delay((int)(1000 * 0.1f), cancellationToken: token);

        _renderer.color = Color.white;
    }

    // 回復
    public virtual void HealHP(int value)
    {
        // 現在のHPと回復量が最大HP未満なら
        if(_hitPoint.Value + value < maxHP)
        {
            _hitPoint.Value += value;
        }
        // 以上なら
        else
        {
            _hitPoint.Value = maxHP;
        }
    }

    // ノックバック
    public virtual void KnockBack(Vector2 damagedPosi, float power)
    {
        if(! allowKnickBack) return;

        var damageDir = (damagedPosi - (Vector2)transform.position).normalized;

        transform.Translate(damageDir * -0.25f);
    }

    // 死亡処理
    public virtual void Die()
    {
        subject_OnDie.OnNext((this, 1));

        Destroy(gameObject);
    }

    protected virtual void OnDestroy()
    {
        foreach (var cts in activeStatusEffects.Values)
        {
            cts.Cancel();
            cts.Dispose();
        }
        activeStatusEffects.Clear();
        activeStatusTypeCounts.Clear();
    }
}
