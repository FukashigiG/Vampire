using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_KnifeCtrler : MonoBehaviour
{
    KnifeData_RunTime knifeData;

    protected float speed;
    protected float lifeTime;
    protected int power;

    // �i�C�t��������Ԃ�������
    bool isBoosted;

    protected virtual void Start()
    {

    }

    //�������p���]�b�g
    public void Initialize(float s, KnifeData_RunTime _knifeData)
    {
        knifeData = _knifeData;

        var renderer = GetComponent<SpriteRenderer>();

        renderer.sprite = _knifeData.sprite_Defolme;
        renderer.color = _knifeData.color;

        speed = s;

        power = knifeData.power;

        lifeTime = 1;
    }

    protected virtual void FixedUpdate()
    {
        // �i��
        transform.Translate(Vector2.up * (speed * 0.2f) * Time.fixedDeltaTime);

        // ����
        lifeTime -= Time.fixedDeltaTime;
        if(lifeTime <= 0 ) Destroy(this.gameObject);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        // ���������������̂��_���[�W���󂯂���̂�������_���[�W��^����
        if(collision.TryGetComponent(out Base_MobStatus ms))
        {
            bool shouldDestroyThis = true;

            int damagePoint = power;

            // �i�C�t��������ԂȂ�A�_���[�W�𑝉����A������ʂ𔭓�������
            if (isBoosted)
            {
                damagePoint += knifeData.elementPower;

                // �i�C�t�ɓ���\�͂��ݒ肳��Ă����ꍇ�̏���
                foreach (var SpEffect in knifeData.specialEffects)
                {
                    if (SpEffect != null)
                    {
                        // �q�b�g���̓��ꏈ�������s
                        // ����̃X�e�[�^�X�A�����̃|�W�V�����ƃi�C�t�f�[�^��n��
                        SpEffect.OnHitSpecialEffect(ms, transform.position, knifeData);

                        // �ђʂ�������Ă���Ȃ�
                        if (SpEffect.dontDestroyBullet == true) shouldDestroyThis = false;
                        // �h�䖳����������Ă���Ȃ�
                        if (SpEffect.ignoreDefence == true) damagePoint += ms.defence / 4; // �h��͕�����悹���邱�ƂŎ�������
                        // �N���e�B�J���Ȃ�
                        if (SpEffect.critical == true) damagePoint *= 2;
                    }
                }
            }

            ms?.GetAttack((int)((damagePoint + speed * 0.75f) / 2), transform.position);

            Instantiate(knifeData.hitEffect, transform.position, Quaternion.identity);

            if (shouldDestroyThis) Destroy(this.gameObject);
        }
    }
}
