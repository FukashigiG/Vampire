using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class Base_KnifeCtrler : MonoBehaviour
{
    KnifeData_RunTime knifeData;

    protected float speed;
    protected float lifeTime;
    protected int power;
    protected int elementPower;

    // ナイフが強化状態かを示す
    bool isBoosted = false;

    bool isActive = true;

    protected virtual void Start()
    {

    }

    //初期化用メゾット
    public void Initialize(float s, KnifeData_RunTime _knifeData, PlayerStatus status, bool boost = false)
    {
        knifeData = _knifeData;

        var renderer = GetComponent<SpriteRenderer>();

        renderer.sprite = _knifeData.sprite;
        //renderer.color = _knifeData.color;

        speed = s;

        power = knifeData.power;
        elementPower = knifeData.elementPower;

        lifeTime = 1;

        isBoosted = boost;

        // ブースト状態でないなら、属性値が半減
        if(!isBoosted) elementPower /= 2;

        //Debug.Log($"{_knifeData.abilities.Count}個のアビリティ");

        // ナイフに特殊能力が設定されていた場合の処理
        foreach (var ability in knifeData.abilities)
        {
            if (ability != null)
            {
                // ブースト状態でないなら、発動率半減
                if (!isBoosted) ability.abilityLogic.probability_Percent /= 2;

                // ヒット時の特殊処理を実行
                // 相手のステータス、自分のポジションとナイフデータを渡す
                ability.OnThrown(status, this.gameObject, knifeData);

            }
        }
    }

    protected virtual void FixedUpdate()
    {
        // 進む
        transform.Translate(Vector2.up * (speed * 0.2f) * Time.fixedDeltaTime);

        // 既にアクティブ状態でないならReturn
        if (!isActive) return;

        // 寿命
        lifeTime -= Time.fixedDeltaTime;
        if(lifeTime <= 0 ) DisappearAndDestroy(false).Forget();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(! isActive) return;

        // もし当たったものがダメージを受けるものだったらダメージを与える
        if(collision.TryGetComponent(out EnemyStatus ms))
        {
            bool shouldDestroyThis = true;
            bool is_ignoreDefence = false;
            bool is_critical = false;

            

            // ナイフに特殊能力が設定されていた場合の処理
            foreach (var ability in knifeData.abilities)
            {
                if (ability != null)
                {
                    // ブースト状態でないなら、発動率半減
                    if (!isBoosted) ability.abilityLogic.probability_Percent /= 2;

                    // ヒット時の特殊処理を実行
                    // 相手のステータス、自分のポジションとナイフデータを渡す
                    ability.OnHit(ms, this.gameObject, knifeData);

                    // 貫通が許可されているなら
                    if (ability.abilityLogic.dontDestroyBullet == true) shouldDestroyThis = false;
                    // クリティカルが許可されているなら
                    if (ability.abilityLogic.critical == true) is_critical = true;
                    // 防御無視が許可されているなら
                    if (ability.abilityLogic.ignoreDefence == true) is_ignoreDefence = true;

                }
            }

            int DMG = (int)((power + speed * 0.75f) / 2);

            if (shouldDestroyThis)
            {
                this.transform.parent = collision.transform;

                DisappearAndDestroy(true).Forget();
            }

            ms?.GetAttack(DMG, elementPower, transform.position, is_critical, is_ignoreDefence);

            Instantiate(knifeData.hitEffect, transform.position, Quaternion.identity);
        }
    }

    async UniTask DisappearAndDestroy(bool isHit)
    {
        if(isHit) speed = 0;

        isActive = false;

        GetComponent<Collider2D>().enabled = false;

        var token = this.GetCancellationTokenOnDestroy();

        if (isHit) await UniTask.Delay(1000, cancellationToken: token);

        await GetComponent<SpriteRenderer>().DOFade(0, 0.5f).ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, cancellationToken: token);

        Destroy(this.gameObject);
    }
}
