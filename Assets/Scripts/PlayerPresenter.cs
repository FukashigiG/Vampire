using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;

public class PlayerPresenter : MonoBehaviour
{
    PlayerStatus status;

    [SerializeField] UI_PlayerHPGauge playerHPGauge;
    [SerializeField] UI_PlayerAbilityCharge playerAbilityCharge;
    [SerializeField] ShowPlayerHandState showHandState;
    [SerializeField] RectTransform iconArea;

    [SerializeField] Image treasureActImage;

    [SerializeField] Image expGauge;

    [SerializeField] GameObject iconPrefab;

    Dictionary<Base_StatusEffectData, GameObject> dictionaly_SED_Icon = new();

    // Start is called before the first frame update
    void Start()
    {
        // プレイヤーのステータスを取得
        status = GetComponent<PlayerStatus>();

        // HP変動を購読、ゲージ更新
        status.hitPoint.Subscribe(value =>
        {
            playerHPGauge.SetGauge(value, status.maxHP);

        }).AddTo(this);
        
        // 手持ちにナイフが加えられるのを購読、
        status.attack.hand_RC.ObserveAdd().Subscribe(knifeData=>
        {
            var sprite = knifeData.Value.sprite;

            showHandState.AddedKnifeInHand(sprite, knifeData.Index);

        }).AddTo(this);

        // 手持ちからナイフが投げられる、取り除かれるのを購読
        status.attack.hand_RC.ObserveRemove().Subscribe(knifeData =>
        {
            showHandState.RemoveKnifeInHand(knifeData.Index);

        }).AddTo(this);

        status.attack.hand_RC.ObserveReset().Subscribe(_ =>
        {
            showHandState.ResetAll();

        }).AddTo(this);

        // 経験値変動を購読、ゲージを更新
        status.exp.Subscribe(value =>
        {
            float ratio = (float)value / (float)status.requiredEXP_LvUp;

            expGauge.fillAmount = ratio;
            

        }).AddTo(this);

        // 状態変化を購読、アイコンを生成・表示
        status.activeStatusTypeCounts.ObserveAdd().Subscribe(x =>
        {
            GameObject obj = Instantiate(iconPrefab, iconArea);

            obj.GetComponent<Image>().sprite = x.Key.icon;

            dictionaly_SED_Icon.Add(x.Key, obj);

        }).AddTo(this);

        // 状態変化終了を購読、アイコンを破棄
        status.activeStatusTypeCounts.ObserveRemove().Subscribe(x =>
        {
            if (dictionaly_SED_Icon.ContainsKey(x.Key))
            {
                Destroy(dictionaly_SED_Icon[x.Key]);

                dictionaly_SED_Icon.Remove(x.Key);
            }

        }).AddTo(this);

        // 秘宝の発動を購読、表示
        Base_TreasureData.onAct.Subscribe(x =>
        {
            treasureActImage.sprite = x.sprite;

        }).AddTo(this);

        // キャラアビリティチャージ変動を購読、ゲージを更新
        status.attack.abilityChargeValue.Subscribe(x =>
        {
            float ratio = (float)x / (float)status.attack.charaAbility.requireChargeValue;

            playerAbilityCharge.SetGauge(ratio);

        }).AddTo(this);
    }
}
