using UnityEngine;

[CreateAssetMenu(fileName = "NewTips", menuName = "Game Data/Tips")]

public class Tips : ScriptableObject
{
    [field: SerializeField] public string Title {  get; private set; }
    [field: SerializeField, TextArea] public string txt {  get; private set; }
}
