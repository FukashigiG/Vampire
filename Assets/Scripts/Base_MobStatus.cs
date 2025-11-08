using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

public enum StatusEffectType
{
    DefenceBuff,
    PowerBuff,
    MoveSpeedBuff,
    DefenceDebuff,
    PowerDebuff,
    MoveSpeedDebuff,
    Blaze,
    Freeze,
    Ghost
}

public class Base_MobStatus : MonoBehaviour, IDamagable
{
    public int maxHP {  get; protected set; }

    protected ReactiveProperty<int> _hitPoint = new ReactiveProperty<int>();
    public ReadOnlyReactiveProperty<int> hitPoint => _hitPoint.ToReadOnlyReactiveProperty();

    public int defence { get {  return (int)(base_Defence * (1f + (enhancementRate_Defence / 100f))); } }
    public int power { get { return (int)(base_Power * (1f + (enhancementRate_Power / 100f))); } }
    public int moveSpeed { get { return (int)(base_MoveSpeed * (1f + (enhancementRate_MoveSpeed / 100f))); } }

    protected int base_Defence;
    protected int base_Power;
    protected int base_MoveSpeed;

    public int enhancementRate_Defence = 0;
    public int enhancementRate_Power = 0;
    public int enhancementRate_MoveSpeed = 0;

    public bool actable { get; protected set; }

    protected Subject<(Vector2 position, int amount)> subject_OnDamaged = new Subject<(Vector2, int)>();
    protected static Subject<(Base_MobStatus status, StatusEffectType type, float duration, int amount)> subject_OnGetStatusEffect = new Subject<(Base_MobStatus, StatusEffectType, float, int)>();
    protected static Subject<(Base_MobStatus status , int value)> subject_OnDie = new Subject<(Base_MobStatus, int)>();
    /*static にすることで、どの Enemy インスタンスからでもこのSubjectにアクセスし、
     * イベントを発行できるようになる
     * また、プレイヤー側で単一のSubjectを購読するだけで、
     * 全ての敵の撃破イベントをキャッチできる*/
    //ゲーム終了時にはGameAdminにDisposeされる

    Dictionary<string, CancellationTokenSource> activeStatusEffects = new();
    Dictionary<StatusEffectType, int> activeStatusTypeCounts = new();

    Collider2D _collider;

    protected virtual void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    protected virtual void Start()
    {
        actable = true;
        _collider.enabled = true;
    }

    // 状態変化効果を適用する統合メソッド
    public void ApplyStatusEffect(StatusEffectType type, string effectID, float duration, int amount = 0)
    {
        //Debug.Log(type);

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
        subject_OnGetStatusEffect.OnNext((this, type, duration, amount));

        // タスクの実行
        StatusEffectTask(type, effectID, duration, amount, cts).Forget();
    }

    // 状態変化効果の非同期処理
    async UniTask StatusEffectTask(StatusEffectType type, string effectID, float duration, int amount, CancellationTokenSource cts)
    {
        // カウントの追加
        if(! activeStatusTypeCounts.ContainsKey(type)) activeStatusTypeCounts[type] = 0;
        activeStatusTypeCounts[type]++;

        // 事前処理：効果を適用する
        switch (type)
        {
            case StatusEffectType.MoveSpeedBuff:
                enhancementRate_MoveSpeed += amount;
                break;
            case StatusEffectType.PowerBuff:
                enhancementRate_Power += amount;
                break;
            case StatusEffectType.DefenceBuff:
                enhancementRate_Defence += amount;
                break;
            case StatusEffectType.MoveSpeedDebuff:
                enhancementRate_MoveSpeed -= amount;
                break;
            case StatusEffectType.PowerDebuff:
                enhancementRate_Power -= amount;
                break;
            case StatusEffectType.DefenceDebuff:
                enhancementRate_Defence -= amount;
                break;
            case StatusEffectType.Freeze:
                actable = false;
                Debug.Log("freeze");
                break;
            case StatusEffectType.Ghost:
                Debug.Log("Ghost");
                _collider.enabled = false;
                break;
            case StatusEffectType.Blaze:
                // Blazeはamountを使わず、内部でダメージ計算
                break;
        }

        try
        {
            if(type == StatusEffectType.Blaze)
            {
                // 一定時間ダメージを受け続ける

                float dmg = maxHP / 100f * 5f;

                int tickCount = Mathf.FloorToInt(duration);

                for (int i = 0; i < tickCount; i++)
                {
                    await UniTask.Delay(1000, cancellationToken: cts.Token);

                    TakeDamage((int)dmg);
                }
            }
            else
            {
                // 通常の待ち処理
                await UniTask.Delay((int)(duration * 1000), cancellationToken: cts.Token);
            }
        }
        catch (System.OperationCanceledException)
        {
            Debug.Log($"{type}.effect was cancelled");
        }
        finally
        {
            if (activeStatusTypeCounts.ContainsKey(type))
            {
                // カウントを減らす
                activeStatusTypeCounts[type]--;

                // カウントが0なら削除
                if (activeStatusTypeCounts[type] >= 0) activeStatusTypeCounts.Remove(type);
            }

            // 事後処理：効果を元に戻す
            switch (type)
            {
                case StatusEffectType.MoveSpeedBuff:
                    enhancementRate_MoveSpeed -= amount;
                    break;
                case StatusEffectType.PowerBuff:
                    enhancementRate_Power -= amount;
                    break;
                case StatusEffectType.DefenceBuff:
                    enhancementRate_Defence -= amount;
                    break;
                case StatusEffectType.MoveSpeedDebuff:
                    enhancementRate_MoveSpeed += amount;
                    break;
                case StatusEffectType.PowerDebuff:
                    enhancementRate_Power += amount;
                    break;
                case StatusEffectType.DefenceDebuff:
                    enhancementRate_Defence += amount;
                    break;
                case StatusEffectType.Freeze:

                    // カウントが残ってなければ（全ての効果が切れてれば）戻す
                    if(! activeStatusTypeCounts.ContainsKey(StatusEffectType.Freeze)) actable = true;
                    break;
                case StatusEffectType.Ghost:

                    if (!activeStatusTypeCounts.ContainsKey(StatusEffectType.Ghost)) _collider.enabled = true;
                    break;
            }

            // Dictionaryに自分に宛てたトークンソースが残っているのであれば、それを削除
            if (activeStatusEffects.ContainsKey(effectID) && activeStatusEffects[effectID] == cts)
            {
                activeStatusEffects.Remove(effectID);
            }

            cts.Dispose();
        }

    }

    // 指定した種類の状態異常がアクティブかを返す関数
    public bool IsStatusEffectTypeActive(StatusEffectType type)
    {
        return activeStatusTypeCounts.ContainsKey(type) && activeStatusTypeCounts[type] > 0;
    }


    // 攻撃を受ける処理
    public virtual void GetAttack(int damagePoint, int elementPoint, Vector2 damagedPosi, bool isCritical = false, bool isIgnoreDefence = false)
    {
        // クリティカルなら基礎ダメージ量を2倍
        if(isCritical) damagePoint *= 2;

        // 防御無視でないなら、防御計算
        if (!isIgnoreDefence) damagePoint -= defence / 4;
        // damagePointを0以下にしない
        if (damagePoint < 0) damagePoint = 0;

        // ダメージ計算式
        int damage = damagePoint + elementPoint;

        // 0以下にならないように
        if (damage <= 0) damage = 1;

        TakeDamage(damage);
    }

    // ダメージ処理
    public virtual void TakeDamage(int value)
    {
        if (value > 0) subject_OnDamaged.OnNext((transform.position, value));

        if (value > 0) _hitPoint.Value -= value;

        if (_hitPoint.Value <= 0) Die();
    }

    /*
     *上二つの関数が分かれてるのは、防御計算をしたい場合としたくない場合に両対応するため 
     */

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
        var damageDir = (damagedPosi - (Vector2)transform.position).normalized;

        transform.Translate(damageDir * -1 * power);
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
