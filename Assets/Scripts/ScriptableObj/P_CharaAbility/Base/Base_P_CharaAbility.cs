using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public abstract class Base_P_CharaAbility : ScriptableObject
{
    // 発動に必要なチャージ量
    [field:SerializeField] public int requireChargeValue {  get; private set; }

    Subject<Unit> subject = new Subject<Unit>();

    public abstract void ActivateAbility(PlayerStatus status);
}
