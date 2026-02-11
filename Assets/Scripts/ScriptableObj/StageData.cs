using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewsStageData", menuName = "Game Data/StageData")]
public class StageData : ScriptableObject
{
    // 各ステージ情報を保持するスクリプタブルオブジェクト
    // それぞれの変数の中身の外部アクセスは参照のみ許可される

    // ステージ名
    [field: SerializeField] public string stageName {  get; private set; }

    // ステージのランク・難易度
    [field: SerializeField, Range(1, 3)] public int stageRank {  get; private set; }

    // 出現する敵のリスト
    [field: SerializeField] public List<EnemyData> enemyList {  get; private set; }

    // 起こるイベントのリスト
    [field: SerializeField] public List<GameEventData> eventList {  get; private set; }

    // ボスの情報
    [field: SerializeField] public EnemyData bossEnemy {  get; private set; }

    // ステージの地面の画像
    [field: SerializeField] public Sprite groungSprite { get; private set; }
}
