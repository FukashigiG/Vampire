using UnityEngine;

public class DI_StageWarp : Base_DropItemCtrler
{
    protected override void TriggerAction()
    {
        GameEventDirector.Instance.Trigger_StageWarp();
    }
}
