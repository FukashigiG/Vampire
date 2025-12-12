using UnityEngine;

[CreateAssetMenu(fileName = "NewFieldLogic", menuName = "Game Data/FieldLogic/Damage")]
public class FEL_Damage : Base_FieldEffectLogic
{
    public override void OnApplyEffect(Base_MobStatus status)
    {
        
    }

    public override void OnRemoveEffect(Base_MobStatus status)
    {
        
    }

    public override void OnSecondEffect(Base_MobStatus status, int effectPoint, Vector2 fieldCenter)
    {
        status.GetAttack(0, effectPoint, fieldCenter);
    }
}
