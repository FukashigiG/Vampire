using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[CreateAssetMenu(fileName = "NewHSpE", menuName = "Game Data/HSpE/Electric")]
public class HSpE_Electric : BaseHSpE
{
    public float magnification;
    public float radius;
    public LayerMask targetLayer;
    [SerializeField] GameObject effect;

    public override void OnHitSpecialEffect(Base_MobStatus status, Vector2 posi, KnifeData_RunTime knifeData)
    {
        // ���͂̓G�ɏ��_���[�W

        Collider2D[] hits = Physics2D.OverlapCircleAll(posi, radius, targetLayer);

        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent(out Base_MobStatus ms))
            {
                // ���������{�l�ɂ͒ǉ��_���[�W�͔������Ȃ�
                if(ms != status) ms.GetAttack((int)(knifeData.power * magnification), posi);

                Instantiate(effect, hit.transform.position, Quaternion.identity);
            }
        }
    }
}
