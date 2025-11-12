using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public abstract class Base_P_CharaAbility : MonoBehaviour
{
    Subject<Unit> subject = new Subject<Unit>();

    protected abstract void ActivateAbility(PlayerStatus status);
}
