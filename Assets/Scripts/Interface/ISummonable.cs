using System;
using UnityEngine;

public interface ISummonable
{
    // ↓abstractが無い状態

    static IObservable<ISummonable> onAwake { get; }
    static IObservable<ISummonable> onDestroy { get; }

    // 寿命
    float lifeTime { get; set; }
}
