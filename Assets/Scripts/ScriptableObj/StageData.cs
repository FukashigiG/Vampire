using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewsStageData", menuName = "Game Data/StageData")]
public class StageData : ScriptableObject
{


    [field: SerializeField] public string stageName {  get; private set; }
    [field: SerializeField, Range(1, 3)] public int stageRank {  get; private set; }
    [field: SerializeField] public List<EnemyData> enemyList {  get; private set; }
    [field: SerializeField] public List<GameEventData> eventList {  get; private set; }
}
