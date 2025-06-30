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
    /*static �ɂ��邱�ƂŁA�ǂ� Enemy �C���X�^���X����ł�����Subject�ɃA�N�Z�X���A
     * �C�x���g�𔭍s�ł���悤�ɂȂ�
     * �܂��A�v���C���[���ŒP���Subject���w�ǂ��邾���ŁA
     * �S�Ă̓G�̌��j�C�x���g���L���b�`�ł���*/
    //�Q�[���I�����ɂ�GameAdmin��Dispose�����

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
