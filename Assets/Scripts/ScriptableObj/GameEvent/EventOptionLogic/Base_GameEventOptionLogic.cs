using UnityEngine;

public abstract class Base_GameEventOptionLogic : ScriptableObject
{
    [field: SerializeField] public string eventOptionText {  get; private set; }

    [SerializeField] string privateMemo;

    public abstract void Action();
}
