using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewsStageData", menuName = "Game Data/StageData")]
public class StageData : ScriptableObject
{
    [field: SerializeField] public string stageName {  get; private set; }
    [field: SerializeField] public List<EnemyData> enemyList {  get; private set; }
}
