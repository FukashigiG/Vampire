using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class Base_MobStatus : MonoBehaviour, IDamagable, IDebuffable
{
    public int hitPoint {  get; protected set; }
    public int defence {  get; protected set; }

    public float moveSpeed { get; protected set; }

    public static Subject<int> onDie = new Subject<int>();
    /*static にすることで、どの Enemy インスタンスからでもこのSubjectにアクセスし、
     * イベントを発行できるようになる
     * また、プレイヤー側で単一のSubjectを購読するだけで、
     * 全ての敵の撃破イベントをキャッチできる*/
    //ゲーム終了時にはGameAdminにDisposeされる

    protected virtual void Start()
    {
        
    }

    public virtual void TakeDamage(int a, Vector2 damagedPosi)
    {
        hitPoint -= a;

        if (hitPoint <= 0) Die();
    }

    public virtual void MoveSpeedDebuff(float duration, float amount)
    {
        moveSpeed *= amount;
    }
    public virtual void AttackDebuff(float duration, float amount)
    {

    }
    public virtual void DefenceDebuff(float duration, float amount)
    {

    }

    protected virtual void Die()
    {
        onDie.OnNext(1);

        Destroy(gameObject);
    }

    protected virtual void OnDestroy()
    {

    }
}
