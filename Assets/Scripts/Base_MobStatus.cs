using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class Base_MobStatus : MonoBehaviour, IDamagable
{
    [SerializeField] int hp;

    public int hitPoint {  get; private set; }

    public static Subject<int> onDie = new Subject<int>();
    /*static �ɂ��邱�ƂŁA�ǂ� Enemy �C���X�^���X����ł�����Subject�ɃA�N�Z�X���A
     * �C�x���g�𔭍s�ł���悤�ɂȂ�
     * �܂��A�v���C���[���ŒP���Subject���w�ǂ��邾���ŁA
     * �S�Ă̓G�̌��j�C�x���g���L���b�`�ł���*/
    //�Q�[���I�����ɂ�GameAdmin��Dispose�����

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
