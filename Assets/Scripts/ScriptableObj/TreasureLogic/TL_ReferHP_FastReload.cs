using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/TreasureLogic/HP_FastReload")]
public class TL_ReferHP_FastReload : Base_TreasureLogic
{
    // プレイヤーのHPが低いほどリロード速度がアップ
    // 途中

    // ナイフ1本あたりの効果割合
    public int maxBonusRatio = 120;

    // 現在の効果量
    int currentBonusValue;

    public override void AddedTrigger(PlayerStatus status)
    {
        // ボーナス値を計算
        currentBonusValue = status.inventory.runtimeKnives.Count;

        // バフを適用
        RiseStatus(currentBonusValue, status);
    }

    public override void RemovedTrigger(PlayerStatus status)
    {
        // 現在のバフを解除
        DeclineStatus(currentBonusValue, status);
    }

    public override void SubscribeToEvent(PlayerStatus status, CompositeDisposable disposables)
    {
        // runtimeKnivesの要素数が変わるたびに呼ばれる
        status.inventory.runtimeKnives.ObserveCountChanged().Subscribe(newCount =>
        {
            RecalculateBonus(newCount, status);
        })
        .AddTo(disposables);
    }

    // ボーナスを再計算して適用するメソッド
    private void RecalculateBonus(int x, PlayerStatus _status)
    {
        // 1. まず現在のバフを一度取り除く
        DeclineStatus(currentBonusValue, _status);

        // 2. 新しいボーナスを計算する
        int newBonus = x;// * xBonusRatio;

        // 3. 新しいバフを適用する
        RiseStatus(newBonus, _status);

        // 4. 現在のボーナス値を更新する
        currentBonusValue = newBonus;
    }

    void RiseStatus(int enhancementPercentage, PlayerStatus status)
    {
        status.enhancementRate_Time_ReloadKnives += enhancementPercentage;
    }

    void DeclineStatus(int enhancementPercentage, PlayerStatus status)
    {
        status.enhancementRate_Time_ReloadKnives -= enhancementPercentage;
    }

    
}
