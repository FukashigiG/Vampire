using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_KnifeCtrler : MonoBehaviour
{
    KnifeData knifeData;

    protected float speed;
    protected float lifeTime;
    protected float power;

    protected virtual void Start()
    {

    }

    //�������p���]�b�g
    public void Initialize(float s, KnifeData _knifeData)
    {
        speed = s;

        knifeData = _knifeData;

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

            // �i�C�t�ɓ���\�͂��ݒ肳��Ă����ꍇ�̏���
            foreach(var SpEffect in knifeData.specialEffects)
            {
                if(SpEffect != null)
                {
                    // �q�b�g���̓��ꏈ�������s
                    SpEffect.OnHitSpecialEffect(ms, transform.position, knifeData);

                    // �ђʂ�������Ă���Ȃ�
                    if(SpEffect.dontDestroyBullet == true) shouldDestroyThis = false;
                    // �h�䖳����������Ă���Ȃ�
                    if (SpEffect.ignoreDefence == true) power += ms.defence / 4; // �h��͕�����悹���邱�ƂŎ�������
                    // �N���e�B�J���Ȃ�
                    if (SpEffect.critical == true) power *= 2;
                }
            }

            ms?.GetAttack((int)((power + speed * 0.75f) / 2), transform.position);

            Instantiate(knifeData.hitEffect, transform.position, Quaternion.identity);

            if (shouldDestroyThis) Destroy(this.gameObject);
        }
    }
}
