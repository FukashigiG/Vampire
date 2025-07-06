using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_KnifeCtrler : MonoBehaviour
{
    [SerializeField] KnifeData knifeData;

    protected float speed;
    protected float lifeTime;
    protected int power;

    protected virtual void Start()
    {
        power = knifeData.power;

        lifeTime = 1;
    }

    //�������p���]�b�g
    public void Initialize(float s)
    {
        speed = s;
    }

    protected virtual void FixedUpdate()
    {
        // �i��
        transform.Translate(Vector2.up * speed * Time.fixedDeltaTime);

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

            ms?.GetAttack(power, transform.position);

            Instantiate(knifeData.hitEffect, transform.position, Quaternion.identity);

            if (shouldDestroyThis) Destroy(this.gameObject);
        }
    }
}
