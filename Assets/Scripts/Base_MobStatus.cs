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
    /*static �ɂ��邱�ƂŁA�ǂ� Enemy �C���X�^���X����ł�����Subject�ɃA�N�Z�X���A
     * �C�x���g�𔭍s�ł���悤�ɂȂ�
     * �܂��A�v���C���[���ŒP���Subject���w�ǂ��邾���ŁA
     * �S�Ă̓G�̌��j�C�x���g���L���b�`�ł���*/
    //�Q�[���I�����ɂ�GameAdmin��Dispose�����

    // �e�f�o�t�����̃L�����Z���ɕK�v�ȃg�[�N���\�[�X
    CancellationTokenSource moveSpeedDbfCts = new CancellationTokenSource();
    CancellationTokenSource powerDbfCts = new CancellationTokenSource();
    CancellationTokenSource defenceDbfCts = new CancellationTokenSource();
    CancellationTokenSource blazeCts = new CancellationTokenSource();
    CancellationTokenSource freezeCts = new CancellationTokenSource();

    protected virtual void Start()
    {
        actable = true;
    }

    // �U�����󂯂鏈��
    public virtual void GetAttack(int a, Vector2 damagedPosi)
    {
        // �_���[�W�v�Z��
        int damage = a - defence / 4;

        // 0�ȉ��ɂȂ�Ȃ��悤��
        if (damage <= 0) damage = 1;

        TakeDamage(damage, damagedPosi);
    }

    // �_���[�W����
    public virtual void TakeDamage(int a, Vector2 damagedPosi)
    {
        hitPoint -= a;

        if (hitPoint <= 0) Die();
    }

    /*
     *���̊֐���������Ă�̂́A�h��v�Z���������ꍇ�Ƃ������Ȃ��ꍇ�ɗ��Ή����邽�� 
     */

    // �m�b�N�o�b�N
    public virtual void KnockBack(Vector2 damagedPosi, float power)
    {
        var damageDir = (damagedPosi - (Vector2)transform.position).normalized;

        transform.Translate(damageDir * -1 * power * (1 / (1 + weight)));
    }


    public void MoveSpeedDebuff(float duration, float amount)
    {
        // ���s�̃g�[�N���\�[�X���L�����Z��
        moveSpeedDbfCts?.Cancel();

        // �g�[�N���\�[�X��V�������̂ɍ����ւ�
        moveSpeedDbfCts = new CancellationTokenSource();

        // ���s
        MSDbfTask(duration, amount, moveSpeedDbfCts.Token).Forget();
    }

    // �ړ����x�f�o�t
    async public virtual UniTask MSDbfTask(float duration, float amount, CancellationToken token)
    {
        moveSpeed *= amount;

        try
        {
            await UniTask.Delay((int)(duration * 1000), cancellationToken: token);

            // �w�莞�ԑ҂��I�������
            moveSpeed /= amount;
        }
        catch
        {
            // ��O����

            Debug.Log("MoveSpeedDebuff was canceled.");

            return;
        }
        finally
        {
            
        }
    }


    public void PowerDebuff(float duration, float amount)
    {
        // ���s�̂��L�����Z��
        powerDbfCts?.Cancel();

        // �g�[�N���\�[�X��V�������̂ɍ����ւ�
        powerDbfCts = new CancellationTokenSource();

        PDbfTask(duration, amount, powerDbfCts.Token).Forget();
    }

    // �̓f�o�t
    async public virtual UniTask PDbfTask(float duration, float amount, CancellationToken token)
    {
        power = (int)(power * amount);

        try
        {
            await UniTask.Delay((int)(duration * 1000), cancellationToken: token);

            // �w�莞�ԑ҂��I�������
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
        // �L�����Z��
        defenceDbfCts?.Cancel();

        // �V�������̂�
        defenceDbfCts = new CancellationTokenSource();

        // ���s
        DDbfTask(duration, amount, defenceDbfCts.Token).Forget();
    }

    // �h��f�o�t
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
