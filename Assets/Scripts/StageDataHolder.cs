using System;
using UnityEngine;

[CreateAssetMenu(fileName = "StageDataHolder", menuName = "Game Data/StageDataHolder")]
public class StageDataHolder : ScriptableObject
{
    [Header("このSOはタイトル - 戦闘シーン間で情報を共有するためのものです")]

    [SerializeField] bool updatable;
    [field: SerializeField] public PlayerCharaData selectedChara {  get; private set; }

    [field: SerializeField] public bool isEndless {  get; private set; }

    public void SetData(PlayerCharaData charaData, bool _isEndless)
    {
        if(! updatable) return;

        selectedChara = charaData;

        isEndless = _isEndless;
    }
}
