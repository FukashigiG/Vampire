using UnityEngine;

public class Base_GameEvent : ScriptableObject
{
    [field: SerializeField] public string eventTitle {  get; private set; }
    [field: SerializeField, Multiline(4)] public string description {  get; private set; }
}
