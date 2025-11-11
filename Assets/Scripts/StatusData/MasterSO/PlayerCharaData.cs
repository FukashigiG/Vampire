using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerCharaData", menuName = "Game Data/Player Chara Data")]
public class PlayerCharaData : ScriptableObject
{
    // プレイヤーキャラクターの各ステータスをまとめたもの
    // このスクリプトはUnityエディタ上に登録するためのものであり、ゲーム中では_RunTimeがついてるものを扱う
    // これらの変数は外部からの参照と、インスペクター上での書き換えが出来る

    [field: SerializeField] public Sprite sprite { get; private set; }

    [field: SerializeField] public string _name { get; private set; }

    [field: SerializeField] public int hp { get; private set; }
    [field: SerializeField] public int power { get; private set; }
    [field: SerializeField] public int defense { get; private set; }
    [field: SerializeField] public int moveSpeed { get; private set; }
    [field: SerializeField] public int luck { get; private set; }
    [field: SerializeField] public int eyeSight { get; private set; }
    [field: SerializeField] public int limit_DrawKnives { get; private set; }

    [field: SerializeField] public KnifeData[] initialKnives { get; private set; }
    [field: SerializeField] public Base_TreasureData[] initialTreasures { get; private set; }

    [field: SerializeField] public Element masteredElement { get; private set; }

    [field: SerializeField] public GameObject prefab { get; private set; }
}
