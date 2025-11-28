using UnityEngine;

public class DI_Heal : Base_DropItemCtrler
{
    [SerializeField] int healAmount = 20;

    protected override void TriggerAction()
    {
        PlayerController.Instance._status.HealHP(healAmount);
    }
}
