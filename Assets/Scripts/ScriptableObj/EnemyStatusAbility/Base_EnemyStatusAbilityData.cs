using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public abstract class Base_EnemyStatusAbilityData : ScriptableObject
{
    // 敵ステータス効果スクリプトの基礎部分

    [field: SerializeField] public string skillName {  get; set; }
    [field: SerializeField] public string description {  get; set; }

    public abstract void ApplyAbility(EnemyStatus status, CompositeDisposable disposables);

}
