using UnityEngine;

public class DI_InvokeGameEvent : Base_DropItemCtrler
{
    [SerializeField] GameEventData gameEventData;

    protected override void TriggerAction()
    {
        GameEventDirector.Instance.Trigger_Event(gameEventData);
    }
}
