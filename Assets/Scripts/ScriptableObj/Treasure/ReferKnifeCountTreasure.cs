using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/Treasure Data/ReferKnifeCount")]
public class ReferKnifeCountTreasure : Base_TreasureData
{
    // プレイヤーの所持するナイフの数を参照してステータスを上昇させる秘宝

    public enum statusEnum
    {
        power, diffence, speed, luck, eyeSight
    }

    // 効果対象
    [SerializeField] statusEnum targetStatus;

    // ナイフ1本あたりの効果割合
    public float xBonusRatio;

    // 現在の効果量
    float currentBonusValue;

    public override void OnAdd(PlayerStatus status)
    {
        // ボーナス値を計算
        currentBonusValue = status.inventory.runtimeKnives.Count * xBonusRatio;

        // バフを適用
        RiseStatus(currentBonusValue, status);
    }

    public override void OnRemove(PlayerStatus status)
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
        float newBonus = x * xBonusRatio;

        // 3. 新しいバフを適用する
        RiseStatus(newBonus, _status);

        // 4. 現在のボーナス値を更新する
        currentBonusValue = newBonus;
    }

    void RiseStatus(float ratio, PlayerStatus status)
    {
        // targetStatusによって効果を適用させるステータスを切り替える
        switch (targetStatus)
        {
            case statusEnum.power:
                status.power *= (1 + ratio);
                break;

            case statusEnum.diffence:
                status.defence *= (1 + ratio);
                break;

            case statusEnum.speed:
                status.moveSpeed *= (1 + ratio);
                break;

            case statusEnum.luck:
                status.luck *= (1 + ratio);
                break;

            case statusEnum.eyeSight:
                status.eyeSight *= (1 + ratio);
                break;
        }
    }

    void DeclineStatus(float ratio, PlayerStatus status)
    {
        // targetStatusによって効果を適用させるステータスを切り替える
        switch (targetStatus)
        {
            case statusEnum.power:
                status.power /= (1 + ratio);
                break;

            case statusEnum.diffence:
                status.defence /= (1 + ratio);
                break;

            case statusEnum.speed:
                status.moveSpeed /= (1 + ratio);
                break;

            case statusEnum.luck:
                status.luck /= (1 + ratio);
                break;

            case statusEnum.eyeSight:
                status.eyeSight /= (1 + ratio);
                break;
        }
    }

    
}
