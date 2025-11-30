using UnityEngine;

public abstract class Base_PlayerItem : ScriptableObject
{
    [field: SerializeField] public string _name { get; private set; }
    [field: SerializeField][TextArea(1, 5)] public string flavortext { get; private set; }

    [field: SerializeField, Range(1, 3)] public int rank { get; private set; }

    [field: SerializeField] public Sprite sprite { get; private set; }

    [field: SerializeField] public Element element { get; private set; }
}
