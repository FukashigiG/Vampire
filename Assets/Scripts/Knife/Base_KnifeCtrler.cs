using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_KnifeCtrler : MonoBehaviour
{
    KnifeData_RunTime knifeData;

    protected float speed;
    protected float lifeTime;
    protected int power;

    // ナイフが強化状態かを示す
    bool isBoosted;

    protected virtual void Start()
    {

    }

    //初期化用メゾット
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

            int damagePoint = power;

            // ナイフが強化状態なら、ダメージを増加し、特殊効果を発動させる
            if (isBoosted)
            {
                damagePoint += knifeData.elementPower;

                // ナイフに特殊能力が設定されていた場合の処理
                foreach (var SpEffect in knifeData.specialEffects)
                {
                    if (SpEffect != null)
                    {
                        // ヒット時の特殊処理を実行
                        // 相手のステータス、自分のポジションとナイフデータを渡す
                        SpEffect.OnHitSpecialEffect(ms, transform.position, knifeData);

                        // 貫通が許可されているなら
                        if (SpEffect.dontDestroyBullet == true) shouldDestroyThis = false;
                        // 防御無視が許可されているなら
                        if (SpEffect.ignoreDefence == true) damagePoint += ms.defence / 4; // 防御力分を上乗せすることで実質無視
                        // クリティカルなら
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
