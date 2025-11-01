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

    public static Subject<(StatusEffectType type, float duration, int amount)> onGetStatusEffect = new Subject<(StatusEffectType, float, int)>();
    public static Subject<(Base_MobStatus status , int value)> onDie = new Subject<(Base_MobStatus, int)>();
    /*static �ɂ��邱�ƂŁA�ǂ� Enemy �C���X�^���X����ł�����Subject�ɃA�N�Z�X���A
     * �C�x���g�𔭍s�ł���悤�ɂȂ�
     * �܂��A�v���C���[���ŒP���Subject���w�ǂ��邾���ŁA
     * �S�Ă̓G�̌��j�C�x���g���L���b�`�ł���*/
    //�Q�[���I�����ɂ�GameAdmin��Dispose�����

    Dictionary<string, CancellationTokenSource> activeStatusEffects = new();

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        actable = true;
    }

    // ��ԕω����ʂ�K�p���铝�����\�b�h
    public void ApplyStatusEffect(StatusEffectType type, string effectID, float duration, int amount = 0)
    {
        // ���łɓ������ʂ��������Ă���ꍇ�́A��x�L�����Z�����Ă���㏑������
        if (activeStatusEffects.ContainsKey(effectID))
        {
            activeStatusEffects[effectID].Cancel();
            activeStatusEffects[effectID].Dispose();
            //activeStatusEffects.Remove(type);

            Debug.Log("already");
        }

        // �V�����g�[�N���\�[�X��p��
        var cts = new CancellationTokenSource();
        activeStatusEffects[effectID] = cts;

        // �C�x���g���s
        // �ϐ��ꎮ��n���̂ŁA���l���w�ǐ悪�ҏW�ł���
        // �i��F����̏�Ԍ��ʂ̎��Ԃ�����������j
        onGetStatusEffect.OnNext((type, duration, amount));

        // �^�X�N�̎��s
        StatusEffectTask(type, effectID, duration, amount, cts).Forget();
    }

    // ��ԕω����ʂ̔񓯊�����
    async UniTask StatusEffectTask(StatusEffectType type, string effectID, float duration, int amount, CancellationTokenSource cts)
    {
        // ���O�����F���ʂ�K�p����
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
                // Blaze��amount���g�킸�A�����Ń_���[�W�v�Z
                break;
        }

        try
        {
            if(type == StatusEffectType.Blaze)
            {
                // ��莞�ԃ_���[�W���󂯑�����

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

            // ���ʏd���������Ȃ��ꍇ�́A�ȉ��̃R�����g�A�E�g���O��

            // Dictionary�Ɏ����Ɉ��Ă��g�[�N���\�[�X���c���Ă���̂ł���΁A������폜
            if (activeStatusEffects.ContainsKey(effectID) && activeStatusEffects[effectID] == cts)
            {
                activeStatusEffects.Remove(effectID);

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
