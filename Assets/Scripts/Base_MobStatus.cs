using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

public enum StatusEffectType
{
    Defence,
    Power,
    MoveSpeed,
    Blaze,
    Freeze
}

public class Base_MobStatus : MonoBehaviour, IDamagable
{
    public int maxHP {  get; protected set; }
    public int hitPoint {  get; protected set; }

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

    public static Subject<(Base_MobStatus status , int value)> onDie = new Subject<(Base_MobStatus, int)>();
    /*static にすることで、どの Enemy インスタンスからでもこのSubjectにアクセスし、
     * イベントを発行できるようになる
     * また、プレイヤー側で単一のSubjectを購読するだけで、
     * 全ての敵の撃破イベントをキャッチできる*/
    //ゲーム終了時にはGameAdminにDisposeされる

    Dictionary<StatusEffectType, CancellationTokenSource> activeStatusEffects = new();

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        actable = true;
    }

    // 状態変化効果を適用する統合メソッド
    public void ApplyStatusEffect(StatusEffectType type, float duration, int amount = 0)
    {
        // すでに同じ効果がかかっている場合は、一度キャンセルしてから上書きする
        if (activeStatusEffects.ContainsKey(type))
        {
            activeStatusEffects[type].Cancel();
            activeStatusEffects[type].Dispose();
            //activeStatusEffects.Remove(type);

            Debug.Log("already");
        }

        // 新しいトークンソースを用意
        var cts = new CancellationTokenSource();
        activeStatusEffects[type] = cts;

        // タスクの実行
        StatusEffectTask(type, duration, amount, cts).Forget();
    }

    // 状態変化効果の非同期処理
    async UniTask StatusEffectTask(StatusEffectType type, float duration, int amount, CancellationTokenSource cts)
    {
        // 事前処理：効果を適用する
        switch (type)
        {
            case StatusEffectType.MoveSpeed:
                enhancementRate_MoveSpeed += amount;
                break;
            case StatusEffectType.Power:
                enhancementRate_Power += amount;
                break;
            case StatusEffectType.Defence:
                enhancementRate_Defence += amount;
                break;
            case StatusEffectType.Freeze:
                actable = false;
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

                    TakeDamage((int)dmg, this.transform.position);
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
            // 事後処理：効果を元に戻す
            switch (type)
            {
                case StatusEffectType.MoveSpeed:
                    enhancementRate_MoveSpeed -= amount;
                    break;
                case StatusEffectType.Power:
                    enhancementRate_Power -= amount;
                    break;
                case StatusEffectType.Defence:
                    enhancementRate_Defence -= amount;
                    break;
                case StatusEffectType.Freeze:
                    actable = true;
                    break;
            }

            // Dictionaryに自分に宛てたトークンソースが残っているのであれば、それを削除
            if (activeStatusEffects.ContainsKey(type) && activeStatusEffects[type] == cts)
            {
                activeStatusEffects.Remove(type);

                Debug.Log("removed");
            }

            cts.Dispose();
        }

    }

    // 攻撃を受ける処理
    public virtual void GetAttack(float a, Vector2 damagedPosi)
    {
        // ダメージ計算式
        int damage = (int)(a - defence / 4);

        // 0以下にならないように
        if (damage <= 0) damage = 1;

        TakeDamage(damage, damagedPosi);
    }

    // ダメージ処理
    public virtual void TakeDamage(int a, Vector2 damagedPosi)
    {
        hitPoint -= a;

        if (hitPoint <= 0) Die();
    }

    /*
     *上二つの関数が分かれてるのは、防御計算をしたい場合としたくない場合に両対応するため 
     */

    // 回復
    public virtual void HealHP(int x)
    {
        hitPoint += x;

        if (hitPoint > maxHP) hitPoint = maxHP;
    }

    // ノックバック
    public virtual void KnockBack(Vector2 damagedPosi, float power)
    {
        var damageDir = (damagedPosi - (Vector2)transform.position).normalized;

        transform.Translate(damageDir * -1 * power);
    }

    public virtual void Die()
    {
        onDie.OnNext((this, 1));

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
    }
}
