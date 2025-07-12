using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHSpE", menuName = "Game Data/HSpE/Blaze")]
public class HSpE_Blaze : BaseHSpE
{
    // “–‚½‚Á‚½“G‚ğ‰Î‰Šó‘Ô‚É‚·‚é

    public float duration;

    public override void OnHitSpecialEffect(Base_MobStatus status, Vector2 posi, KnifeData knifeData)
    {
        status.Blaze(duration);
    }
}
