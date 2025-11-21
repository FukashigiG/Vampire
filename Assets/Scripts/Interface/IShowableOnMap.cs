using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public interface IShowableOnMap
{
    static Subject<Unit> onBirth;
}
