using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UniRx;
using UnityEngine;

public abstract class Base_P_CharaAbility : ScriptableObject
{
    [field: SerializeField] public string abilityName { get; private set; }
    [field: SerializeField, TextArea] public string explanation{ get; private set; }

    // 発動に必要なチャージ量
    [field:SerializeField] public int requireChargeValue {  get; private set; }

    protected Subject<Unit> subject = new Subject<Unit>();

    protected PlayerStatus player;

    // 初期化処理
    public virtual void Initialize(PlayerStatus status)
    {
        player = status;
    }

    // 本効果
    public abstract UniTask ActivateAbility(CancellationToken token);
}
