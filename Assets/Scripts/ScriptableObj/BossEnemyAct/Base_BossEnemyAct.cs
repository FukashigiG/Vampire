using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public abstract class Base_BossEnemyAct : ScriptableObject
{
    [field: SerializeField] public string actionName {  get; private set; }
    [field: SerializeField] public float range_Min {  get; private set; }
    [field: SerializeField] public float range_Max {  get; private set; }

    [SerializeField] protected float delayTime;

    public enum ActType { Move, Attack}
    [field: SerializeField] public ActType actType { get; private set; }


    CancellationToken token;

    // ‚±‚Ì“_‚Å‚Íasync‚Í‹Lq‚µ‚Ä‚È‚­‚Ä‚à‚¢‚¢‚ç‚µ‚¢
    public virtual UniTask Action(Base_EnemyCtrler ctrler)
    {
        return UniTask.CompletedTask;
    }
}
