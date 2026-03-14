using System;
using UnityEngine;

public class PlayerPropsBulletCtrler : MonoBehaviour
{
    [SerializeField] GameObject defaultFx;

    [SerializeField] float speed;

    ParticleSystem fx;

    float lifeTime = 4f;
    float elapsedTime = 0;

    int damagePoint = 0;

    public enum DamageType
    {
        normal, elemental, fusion
    }

    DamageType damageType = DamageType.normal;

    public void Initialize(int damage, float _speed = 5f, GameObject _fx = null, DamageType type = DamageType.elemental)
    {
        damagePoint = damage;
        speed = _speed;

        damageType = type;

        GameObject fxPrefab = null;

        if(_fx != null)
        {
            fxPrefab = _fx;
        }
        else
        {
            fxPrefab = defaultFx;
        }

        fx = Instantiate(fxPrefab, transform.position, transform.rotation, this.transform).GetComponent<ParticleSystem>();
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= lifeTime) Die();

        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out EnemyStatus ms))
        {
            switch (damageType)
            {
                case DamageType.normal:
                    // 物理ダメージ
                    ms.GetAttack(damagePoint, 0, transform.position);
                    break;

                case DamageType.elemental:
                    // 属性ダメージ
                    ms.GetAttack(0, damagePoint, transform.position);
                    break;

                case DamageType.fusion:
                    // 両方で半分づつダメージ
                    ms.GetAttack(damagePoint/2, damagePoint/2, transform.position);
                    break;

                default:
                    // 物理ダメージ
                    ms.GetAttack(damagePoint, 0, transform.position);
                    break;
            }

            Die();
        }
    }

    public void Die()
    {
        fx.transform.parent = null;
        fx.Stop();

        Destroy(this.gameObject);
    }
}
