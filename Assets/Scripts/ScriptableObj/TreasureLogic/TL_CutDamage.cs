using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/TreasureLogic/CutDamage")]
public class TL_CutDamage : Base_TreasureLogic
{
    [SerializeField] int cutAmount;

    System.Func<int, int> filter;

    public override void AddedTrigger(PlayerStatus status)
    {
        base.AddedTrigger(status);

        // 入力された数値をNだけ減らすに固定するフィルタ
        filter = (originalDamage) => originalDamage - cutAmount;

        status.damageFilters.Add(filter);
    }

    public override void RemovedTrigger(PlayerStatus status)
    {
        base.RemovedTrigger(status);

        status.damageFilters.Remove(filter);
    }
}
