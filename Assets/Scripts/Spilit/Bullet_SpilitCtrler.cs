using UnityEngine;

public class Bullet_SpilitCtrler : MonoBehaviour
{
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
            // ‘®«ƒ_ƒ[ƒW
            ms.GetAttack(0, damagePoint, transform.position);

            Destroy(this.gameObject);
        }
    }
}
