using UnityEngine;
using UnityEngine.EventSystems;

public class DI_GetSomeone : Base_DropItemCtrler
{
    protected override void TriggerAction()
    {
        GameEventDirector.Instance.Trigger_GetSome();
    }
}
