using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtrler_Infight : Base_EnemyCtrler
{
    // ‹ßÚŒ^‚Ì“G‚Ì‹““®

    protected override void HandleAI()
    {
        // ƒVƒ“ƒvƒ‹‚ÉÚ‹ß‚·‚é

        //float distance = (target.position - this.transform.position).magnitude;

        Vector2 dir = (target.position - this.transform.position).normalized;

        transform.Translate(dir * _enemyStatus.moveSpeed / 10f * Time.fixedDeltaTime);
    }
}
