using UnityEngine;

public class DI_InvokeGameEvent : Base_DropItemCtrler
{
    protected override void TriggerAction()
    {
        GameEventDirector.Instance.Trigger_Event();
    }
}
