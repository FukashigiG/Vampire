using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class Base_MobStatus : MonoBehaviour, IDamagable
{
    [SerializeField] int hp;

    public int hitPoint {  get; private set; }

    public static Subject<int> onDie = new Subject<int>();
    /*static にすることで、どの Enemy インスタンスからでもこのSubjectにアクセスし、
     * イベントを発行できるようになる
     * また、プレイヤー側で単一のSubjectを購読するだけで、
     * 全ての敵の撃破イベントをキャッチできる*/
    //ゲーム終了時にはGameAdminにDisposeされる

    protected virtual void Start()
    {
        hitPoint = hp;
    }

    public void TakeDamage(int a)
    {
        hitPoint -= a;

        if (hitPoint <= 0) Die();
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
