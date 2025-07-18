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

    //初期化用メゾット
    public void Initialize(float s, KnifeData _knifeData)
    {
        speed = s;

        knifeData = _knifeData;

        power = knifeData.power;

        lifeTime = 1;
    }

    protected virtual void FixedUpdate()
    {
        // 進む
        transform.Translate(Vector2.up * (speed * 0.2f) * Time.fixedDeltaTime);

        // 寿命
        lifeTime -= Time.fixedDeltaTime;
        if(lifeTime <= 0 ) Destroy(this.gameObject);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        // もし当たったものがダメージを受けるものだったらダメージを与える
        if(collision.TryGetComponent(out Base_MobStatus ms))
        {
            bool shouldDestroyThis = true;

            // ナイフに特殊能力が設定されていた場合の処理
            foreach(var SpEffect in knifeData.specialEffects)
            {
                if(SpEffect != null)
                {
                    // ヒット時の特殊処理を実行
                    SpEffect.OnHitSpecialEffect(ms, transform.position, knifeData);

                    // 貫通が許可されているなら
                    if(SpEffect.dontDestroyBullet == true) shouldDestroyThis = false;
                    // 防御無視が許可されているなら
                    if (SpEffect.ignoreDefence == true) power += ms.defence / 4; // 防御力分を上乗せすることで実質無視
                    // クリティカルなら
                    if (SpEffect.critical == true) power *= 2;
                }
            }

            ms?.GetAttack((int)((power + speed * 0.75f) / 2), transform.position);

            Instantiate(knifeData.hitEffect, transform.position, Quaternion.identity);

            if (shouldDestroyThis) Destroy(this.gameObject);
        }
    }
}
