using UnityEngine;

public abstract class Base_FieldEffectLogic : ScriptableObject
{
    public enum Target { player, Enemy }

    [field : SerializeField] public Target effectTarget {  get; private set; }

    public abstract void OnApplyEffect(Base_MobStatus status);
    public abstract void OnRemoveEffect(Base_MobStatus status);

    public abstract void OnSecondEffect(Base_MobStatus statusint, int effectPoint, Vector2 fieldPoint);
}