using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : Base_MobStatus
{
    [SerializeField] Image gauge_EXP;

    [SerializeField] PlayerCharaData playerCharaData;

    [SerializeField] int requiredEXP_LvUp;

    public float moveSpeed {  get; private set; }
    public float defense {  get; private set; }
    public float throwPower { get; private set; }
    public float luck {  get; private set; }

    int exp;


    public Subject<Unit> lvUp = new Subject<Unit>();

    // �w�ǂ̃��C�t�T�C�N�����Ǘ����邽�߂�Disposable
    // ����P�ő�R��Disposable�Ȃ��ɑΉ��\�炵��
    private CompositeDisposable disposables = new CompositeDisposable();

    protected override void Start()
    {
        base.Start();

        //�e�����X�e�[�^�X��PlayerCharaData������
        hitPoint = playerCharaData.hp;
        moveSpeed = playerCharaData.moveSpeed;
        defense = playerCharaData.defense;
        throwPower = playerCharaData.throwPower;
        luck = playerCharaData.luck;

        //�C�ӂ̓G�����񂾂�EXP�Q�b�g
        EnemyStatus.onDie.Subscribe(x => GetEXP(x)).AddTo( disposables );
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
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        disposables.Dispose();
    }
}
