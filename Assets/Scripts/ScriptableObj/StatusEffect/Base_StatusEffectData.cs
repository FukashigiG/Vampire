using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public abstract class Base_StatusEffectData : ScriptableObject
{
    [field: SerializeField] public string effectName {  get; private set; }
    [field: SerializeField] public Sprite icon {  get; private set; }

    // 効果時間中定期的に何かしらの処理をするか
    public virtual bool IsTickingEffect => false;

    // 効果適用時に呼ばれる
    // target = 効果対象, amount = 効果量
    public abstract void Apply(Base_MobStatus target, int amount);
    // 効果終了時に呼ばれる
    public abstract void Remove(Base_MobStatus target, int amount);

    public virtual async UniTask Tick(Base_MobStatus target, float duration, int amount, CancellationToken token)
    {
        // デフォルトでは何もしない
        await UniTask.CompletedTask;
    }
}
