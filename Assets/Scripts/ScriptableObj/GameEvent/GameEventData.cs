using UnityEngine;

[CreateAssetMenu(fileName = "NewGameEvent", menuName = "Game Data/GameEvent")]
public class GameEventData : ScriptableObject
{
    [field: SerializeField] public string eventTitle {  get; private set; }
    [field: SerializeField, Multiline(4)] public string description {  get; private set; }

    [field:SerializeField] public Base_GameEventOptionLogic[] eventOptionLogic {  get; private set; }
}
