using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public abstract class Base_EnemyStatusAbilityData : ScriptableObject, IDiscribing
{
    // 敵ステータス効果スクリプトの基礎部分

    [field: SerializeField] public string _name {  get; private set; }
    [field: SerializeField] public string description {  get; private set; }
    [field: SerializeField] public Sprite icon {  get; private set; }

    public abstract void ApplyAbility(EnemyStatus status, CompositeDisposable disposables);

}
