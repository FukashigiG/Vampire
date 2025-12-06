using UnityEngine;

public abstract class Base_PlayerItem : ScriptableObject
{
    [field: SerializeField] public string _name { get; protected set; }
    [field: SerializeField][TextArea(1, 5)] public string flavortext { get; protected set; }

    [field: SerializeField, Range(1, 3)] public int rank { get; protected set;}
    [field: SerializeField] public Sprite sprite { get; protected set; }

    [field: SerializeField] public Element element { get; protected set; }
}
