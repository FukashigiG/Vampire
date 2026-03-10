using UnityEngine;

public class Bullet_SpilitCtrler : MonoBehaviour
{
    [SerializeField] ParticleSystem fx_Trail;

    [SerializeField] float speed = 5f;

    float lifeTime = 4f;
    float elapsedTime = 0;

    int damagePoint = 0;

    public void Initialize(int damage)
    {
        damagePoint = damage;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= lifeTime) Destroy(gameObject);

        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out EnemyStatus ms))
        {
            // 属性ダメージ
            ms.GetAttack(0, damagePoint, transform.position);

            fx_Trail.transform.parent = null;
            fx_Trail.Stop();

            Destroy(this.gameObject);
        }
    }
}
