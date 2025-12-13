using UnityEngine;

[CreateAssetMenu(fileName = "NewFieldLogic", menuName = "Game Data/FieldLogic/Heal")]
public class FEL_Heal : Base_FieldEffectLogic
{
    public override void OnApplyEffect(Base_MobStatus status)
    {
        
    }

    public override void OnRemoveEffect(Base_MobStatus status)
    {
        
    }

    public override void OnSecondEffect(Base_MobStatus status, int effectPoint, Vector2 fieldCenter)
    {
        status.HealHP(effectPoint);
    }
}
