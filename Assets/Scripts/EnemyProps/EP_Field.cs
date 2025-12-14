using UnityEngine;
using System.Collections.Generic;

public class EP_Field : Base_EnemyProps
{
    List<Base_MobStatus> effectings = new List<Base_MobStatus>();

    Base_FieldEffectLogic effectLogic = null;

    float friquentry = 1f;

    float elapsedTime = 0;

    public float cullentRadius => transform.localScale.x / 2f;

    // 初期化
    public void Initialize_Field(Base_FieldEffectLogic _effectLogic, float radius, int baseDamage, int elementDamage)
    {
        base.Initialize(baseDamage, elementDamage);

        effectLogic = _effectLogic;

        SetScale(radius);
    }

    // 大きさ
    public void SetScale(float radius)
    {
        transform.localScale = new Vector3(radius * 2, radius * 2, 1);
    }

    // 毎効果処理
    private void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > friquentry)
        {
            elapsedTime = 0;

            foreach(var status in effectings)
            {
                effectLogic.OnSecondEffect(status, elementDamage, this.transform.position);
            }
        }
    }

    // 何かしらが入ってきたとき
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Base_MobStatus status = null;

        // それが効果対象なら
        switch (effectLogic.effectTarget)
        {
            case Base_FieldEffectLogic.Target.player:

                if (! collision.TryGetComponent(out PlayerStatus player)) return;
                status = player;
                break;

            case Base_FieldEffectLogic.Target.Enemy:

                if (!collision.TryGetComponent(out EnemyStatus enemy)) return;
                status = enemy;
                break;
        }

        ApplyEffect(status);
    }

    // 何かしらが出てった時
    private void OnTriggerExit2D(Collider2D collision)
    {
        // それが生き物で
        if (!collision.TryGetComponent(out Base_MobStatus status)) return;

        // 効果適用リストに入ってるなら
        if(effectings.Contains(status)) RemoveEffect(status);
    }

    // 対象に効果適用
    void ApplyEffect(Base_MobStatus status)
    {
        effectings.Add(status);

        effectLogic.OnApplyEffect(status);
    }

    // 対象の効果を除外
    void RemoveEffect(Base_MobStatus status)
    {
        effectings.Remove(status);

        effectLogic.OnRemoveEffect(status);
    }
}
