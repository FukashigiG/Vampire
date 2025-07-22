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

    public float defence;
    public float power;
    public float moveSpeed;
    public float weight;

    public bool actable { get; protected set; }

    public static Subject<int> onDie = new Subject<int>();
    /*static �ɂ��邱�ƂŁA�ǂ� Enemy �C���X�^���X����ł�����Subject�ɃA�N�Z�X���A
     * �C�x���g�𔭍s�ł���悤�ɂȂ�
     * �܂��A�v���C���[���ŒP���Subject���w�ǂ��邾���ŁA
     * �S�Ă̓G�̌��j�C�x���g���L���b�`�ł���*/
    //�Q�[���I�����ɂ�GameAdmin��Dispose�����

    Dictionary<StatusEffectType, CancellationTokenSource> activeStatusEffects = new();

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        actable = true;
    }

    // ��ԕω����ʂ�K�p���铝�����\�b�h
    public void ApplyStatusEffect(StatusEffectType type, float duration, float amount = 0)
    {
        // ���łɓ������ʂ��������Ă���ꍇ�́A��x�L�����Z�����Ă���㏑������
        if (activeStatusEffects.ContainsKey(type))
        {
            activeStatusEffects[type].Cancel();
            activeStatusEffects[type].Dispose();
            //activeStatusEffects.Remove(type);

            Debug.Log("already");
        }

        // �V�����g�[�N���\�[�X��p��
        var cts = new CancellationTokenSource();
        activeStatusEffects[type] = cts;

        // �^�X�N�̎��s
        StatusEffectTask(type, duration, amount + 1f, cts).Forget();
    }

    // ��ԕω����ʂ̔񓯊�����
    async UniTask StatusEffectTask(StatusEffectType type, float duration, float amount, CancellationTokenSource cts)
    {
        // ���O�����F���ʂ�K�p����
        switch (type)
        {
            case StatusEffectType.MoveSpeed:
                moveSpeed *= amount;
                break;
            case StatusEffectType.Power:
                power *= amount;
                break;
            case StatusEffectType.Defence:
                defence *= amount;
                break;
            case StatusEffectType.Freeze:
                actable = false;
                break;
            case StatusEffectType.Blaze:
                // Blaze��amount���g�킸�A�����Ń_���[�W�v�Z
                break;
        }

        try
        {
            if(type == StatusEffectType.Blaze)
            {
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
                // �ʏ�̑҂�����
                await UniTask.Delay((int)(duration * 1000), cancellationToken: cts.Token);
            }
        }
        catch (System.OperationCanceledException)
        {
            Debug.Log($"{type}.effect was cancelled");
        }
        finally
        {
            // ���㏈���F���ʂ����ɖ߂�
            switch (type)
            {
                case StatusEffectType.MoveSpeed:
                    moveSpeed /= amount; 
                    break;
                case StatusEffectType.Power:
                    power /= amount; 
                    break;
                case StatusEffectType.Defence:
                    defence /= amount; 
                    break;
                case StatusEffectType.Freeze:
                    actable = true;
                    break;
            }

            // Dictionary�Ɏ����Ɉ��Ă��g�[�N���\�[�X���c���Ă���̂ł���΁A������폜
            if (activeStatusEffects.ContainsKey(type) && activeStatusEffects[type] == cts)
            {
                activeStatusEffects.Remove(type);

                Debug.Log("removed");
            }

            cts.Dispose();
        }

    }

    // �U�����󂯂鏈��
    public virtual void GetAttack(float a, Vector2 damagedPosi)
    {
        // �_���[�W�v�Z��
        int damage = (int)(a - defence / 4);

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

    // ��
    public virtual void HealHP(int x)
    {
        hitPoint += x;

        if (hitPoint > maxHP) hitPoint = maxHP;
    }

    // �m�b�N�o�b�N
    public virtual void KnockBack(Vector2 damagedPosi, float power)
    {
        var damageDir = (damagedPosi - (Vector2)transform.position).normalized;

        transform.Translate(damageDir * -1 * power * (1 / (1 + weight)));
    }

    public virtual void Die()
    {
        onDie.OnNext(1);

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
