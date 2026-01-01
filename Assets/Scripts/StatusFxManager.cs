using UnityEngine;

public class StatusFxManager : SingletonMono<StatusFxManager>
{
    [field: SerializeField] public GameObject fx_Buff {  get; private set; }
    [field: SerializeField] public GameObject fx_Debuff { get; private set; }

    [field: SerializeField] public GameObject fx_Blaze { get; private set; }
    [field: SerializeField] public GameObject fx_Frost { get; private set; }
}
