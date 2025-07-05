using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

public class Base_MobStatus : MonoBehaviour, IDamagable, IDebuffable
{
    public int maxHP {  get; protected set; }
    public int hitPoint {  get; protected set; }
    public int defence {  get; protected set; }
    public int power {  get; protected set; }
    public float moveSpeed { get; protected set; }
    public float weight { get; protected set; }

    public bool actable { get; protected set; }

    public static Subject<int> onDie = new Subject<int>();
    /*static にすることで、どの Enemy インスタンスからでもこのSubjectにアクセスし、
     * イベントを発行できるようになる
     * また、プレイヤー側で単一のSubjectを購読するだけで、
     * 全ての敵の撃破イベントをキャッチできる*/
    //ゲーム終了時にはGameAdminにDisposeされる

    // 各デバフ処理のキャンセルに必要なトークンソース
    CancellationTokenSource moveSpeedDbfCts = new CancellationTokenSource();
    CancellationTokenSource powerDbfCts = new CancellationTokenSource();
    CancellationTokenSource defenceDbfCts = new CancellationTokenSource();
    CancellationTokenSource blazeCts = new CancellationTokenSource();
    CancellationTokenSource freezeCts = new CancellationTokenSource();

    protected virtual void Start()
    {
        actable = true;
    }

    // 攻撃を受ける処理
    public virtual void GetAttack(int a, Vector2 damagedPosi)
    {
        // ダメージ計算式
        int damage = a - defence / 4;

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

    // ノックバック
    public virtual void KnockBack(Vector2 damagedPosi, float power)
    {
        var damageDir = (damagedPosi - (Vector2)transform.position).normalized;

        transform.Translate(damageDir * -1 * power * (1 / (1 + weight)));
    }


    public void MoveSpeedDebuff(float duration, float amount)
    {
        // 現行のトークンソースをキャンセル
        moveSpeedDbfCts?.Cancel();

        // トークンソースを新しいものに差し替え
        moveSpeedDbfCts = new CancellationTokenSource();

        // 実行
        MSDbfTask(duration, amount, moveSpeedDbfCts.Token).Forget();
    }

    // 移動速度デバフ
    async public virtual UniTask MSDbfTask(float duration, float amount, CancellationToken token)
    {
        moveSpeed *= amount;

        try
        {
            await UniTask.Delay((int)(duration * 1000), cancellationToken: token);

            // 指定時間待ち終わったら
            moveSpeed /= amount;
        }
        catch
        {
            // 例外処理

            Debug.Log("MoveSpeedDebuff was canceled.");

            return;
        }
        finally
        {
            
        }
    }


    public void PowerDebuff(float duration, float amount)
    {
        // 現行のをキャンセル
        powerDbfCts?.Cancel();

        // トークンソースを新しいものに差し替え
        powerDbfCts = new CancellationTokenSource();

        PDbfTask(duration, amount, powerDbfCts.Token).Forget();
    }

    // 力デバフ
    async public virtual UniTask PDbfTask(float duration, float amount, CancellationToken token)
    {
        power = (int)(power * amount);

        try
        {
            await UniTask.Delay((int)(duration * 1000), cancellationToken: token);

            // 指定時間待ち終わったら
            power = (int)(power / amount);
        }
        catch
        {
            Debug.Log("PowerDebuff was canceled.");

            return;
        }
        finally
        {

        }
    }


    public void DefenceDebuff(float duration, float amount)
    {
        // キャンセル
        defenceDbfCts?.Cancel();

        // 新しいものに
        defenceDbfCts = new CancellationTokenSource();

        // 実行
        DDbfTask(duration, amount, defenceDbfCts.Token).Forget();
    }

    // 防御デバフ
    async public virtual UniTask DDbfTask (float duration, float amount, CancellationToken token)
    {
        defence = (int)(defence * amount);

        try
        {
            await UniTask.Delay((int)(duration * 1000), cancellationToken: token);

            defence = (int)(defence / amount);
        }
        catch
        {
            Debug.Log("DefenceDebuff was canceled.");
            
            return;
        }
        finally
        {
            
        }
    }

    public virtual void Blaze(float duration)
    {
        blazeCts?.Cancel();

        blazeCts = new CancellationTokenSource();

        BlazeTask(duration, blazeCts.Token).Forget();
    }

    async UniTask BlazeTask(float duration, CancellationToken token)
    {
        try
        {
            float dmg = maxHP / 100f * 5f;

            int num = Mathf.FloorToInt(duration);

            for(int i = 0; i < 5;  i++)
            {
                await UniTask.Delay(1000, cancellationToken: token);

                TakeDamage((int)dmg, this.transform.position);
            }
            
        }
        catch
        {
            return;
        }
        finally
        {

        }
    }

    public virtual void Freeze(float duration)
    {
        freezeCts?.Cancel();

        freezeCts = new CancellationTokenSource();

        FreezeTask(duration, freezeCts.Token).Forget();
    }

    async UniTask FreezeTask(float duration, CancellationToken token)
    {
        try
        {
            actable = false;

            await UniTask.Delay((int)(duration * 1000), cancellationToken: token);

            actable = true;
        }
        catch
        {
            return ;
        }
        finally
        {
            
        }
    }

    public virtual void Die()
    {
        onDie.OnNext(1);

        Destroy(gameObject);
    }

    protected virtual void OnDestroy()
    {
        moveSpeedDbfCts?.Cancel();
        powerDbfCts?.Cancel();
        defenceDbfCts?.Cancel();
        blazeCts?.Cancel();
        freezeCts?.Cancel();

        moveSpeedDbfCts.Dispose();
        powerDbfCts.Dispose();
        defenceDbfCts.Dispose();
        blazeCts.Dispose();
        freezeCts.Dispose();
    }
}
