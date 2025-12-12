using UnityEngine;

[CreateAssetMenu(fileName = "NewFieldLogic", menuName = "Game Data/FieldLogic/Buff")]
public class FEL_Buff : Base_FieldEffectLogic
{
    public override void OnApplyEffect(Base_MobStatus status)
    {
        status.enhancementRate_MoveSpeed += 100;
    }

    public override void OnRemoveEffect(Base_MobStatus status)
    {
        status.enhancementRate_MoveSpeed -= 100;

    }

    public override void OnSecondEffect(Base_MobStatus status, int effectPoint, Vector2 fieldCenter)
    {
        
    }
}
