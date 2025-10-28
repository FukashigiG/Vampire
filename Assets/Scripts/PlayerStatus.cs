using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : Base_MobStatus
{
    [SerializeField] Image gauge_EXP;
    [SerializeField] Image gauge_HP;

    public PlayerCharaData playerCharaData;

    public PlayerInventory inventory { get; private set; }
    public PlayerAttack attack { get; private set; }

    [SerializeField] int requiredEXP_LvUp;

    public float luck;
    public float eyeSight;

    int exp;

    bool isInvincible;

    public Subject<Unit> lvUp = new Subject<Unit>();

    // �w�ǂ̃��C�t�T�C�N�����Ǘ����邽�߂�Disposable
    // ����P�ő�R��Disposable�Ȃ��ɑΉ��\�炵��
    private CompositeDisposable disposables = new CompositeDisposable();

    protected override void Awake()
    {
        base.Awake();

        inventory = GetComponent<PlayerInventory>();
        attack = GetComponent<PlayerAttack>();

        //�e�����X�e�[�^�X��PlayerCharaData������
        maxHP = playerCharaData.hp;
        hitPoint = maxHP;
        base_Power = playerCharaData.power; //�@�v���C���[��power�̓i�C�t�̑��x�����肷��
        base_Defence = playerCharaData.defense;
        base_MoveSpeed = playerCharaData.moveSpeed;

        luck = playerCharaData.luck;
        eyeSight = playerCharaData.eyeSight;
    }

    protected override void Start()
    {
        base.Start();

        // �C���x���g���ɏ��������i�C�t�Ɣ���ǉ�
        foreach (var x in playerCharaData.initialKnives)
        {
            inventory.AddKnife(x);
        }
        foreach (var y in playerCharaData.initialTreasures)
        {
            inventory.AddTreasure(y);
        }

        //�C�ӂ̓G�����񂾂�EXP�Q�b�g
        EnemyStatus.onDie.Subscribe(x => GetEXP(x.value)).AddTo( disposables );
    }

    // exp�l��
    public void GetEXP(int x)
    {
        exp += x;

        // �o���l�ʂ����ɒB���Ă����烌�x���A�b�v
        if (exp >= requiredEXP_LvUp) LvUp();

        // �o���l�Q�[�WUI�X�V
        gauge_EXP.fillAmount = (float)exp / (float)requiredEXP_LvUp;
    }

    // ���x���A�b�v
    void LvUp()
    {
        while (exp >= requiredEXP_LvUp)
        {
            exp -= requiredEXP_LvUp ;

            requiredEXP_LvUp += 1;
        }

        lvUp.OnNext(Unit.Default);

        // �i�C�t�l���C�x���g�����s
        GameEventDirector.Instance.TriggerEvent(GameEventDirector.Events.getKnife);
    }

    // �U�����󂯂鏈��
    public override void GetAttack(float a, Vector2 damagedPosi)
    {
        if (isInvincible) return;

        base.GetAttack(a, damagedPosi);

        BeInvincible(1f).Forget();
    }

    // �_���[�W���󂯂鏈��
    public override void TakeDamage(int a, Vector2 damagedPosi)
    {
        base.TakeDamage(a, damagedPosi);

        gauge_HP.fillAmount = (float)hitPoint / (float)maxHP;
    }

    // �w��b���ԂނĂ��ɂȂ�
    async UniTask BeInvincible(float sec)
    {
        try
        {
            isInvincible = true;

            await UniTask.Delay((int)(sec * 1000));

            isInvincible = false;
        }
        catch
        {

        }
        finally
        {

        }
    }

    public override void Die()
    {
        onDie.OnNext((this, 1));

        this.gameObject.SetActive(false);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        disposables.Dispose();
    }
}
