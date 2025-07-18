using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyKnifeCtrler : Base_KnifeCtrler
{
    [SerializeField] LayerMask targetLayer;

    [SerializeField] float radius;

    protected override void Start()
    {
        base.Start();

        //ˆê’è”ÍˆÍ“à‚Ì“G‚ğ”z—ñ‚ÉŠi”[
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, targetLayer);

        // ƒŠƒXƒg‚É‚ ‚é“G‚»‚ê‚¼‚ê‚É‘Î‚µ‚Ä
        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent(out IDamagable i_d))
            {
                // power‚Ì”¼•ª‚Ìƒ_ƒ[ƒW‚ğ—^‚¦‚é
                i_d.TakeDamage((int)(power / 2), transform.position);
            }
        }
    }
}
