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
    [SerializeField] Image playerEXPGauge;
    [SerializeField] ShowPlayerHandState showHandState;
    [SerializeField] RectTransform iconArea;

    [SerializeField] Image treasureActImage;

    [SerializeField] Image charaAbilityChargeValue;

    [SerializeField] GameObject iconPrefab;

    Dictionary<Base_StatusEffectData, GameObject> dictionaly_SED_Icon = new();

    // Start is called before the first frame update
    void Start()
    {
        status = GetComponent<PlayerStatus>();

        status.hitPoint.Subscribe(value =>
        {
            playerHPGauge.SetGauge(value, status.maxHP);

        }).AddTo(this);

        status.attack.onReload.Subscribe(list =>
        {
            var sprits = list.Select(knifeData => knifeData.sprite).ToList();

            showHandState.ShowReloadResult(sprits);

        }).AddTo(this);

        status.attack.onThrowKnife.Subscribe(value =>
        {
            showHandState.Thrown();

        }).AddTo(this);

        status.exp.Subscribe(value =>
        {
            playerEXPGauge.fillAmount = (float)value / (float)status.requiredEXP_LvUp;

        }).AddTo(this);

        status.activeStatusTypeCounts.ObserveAdd().Subscribe(x =>
        {
            if (! dictionaly_SED_Icon.ContainsKey(x.Key))
            {
                
            }

            GameObject obj = Instantiate(iconPrefab, iconArea);

            obj.GetComponent<Image>().sprite = x.Key.icon;

            dictionaly_SED_Icon.Add(x.Key, obj);

        }).AddTo(this);

        status.activeStatusTypeCounts.ObserveRemove().Subscribe(x =>
        {
            if (dictionaly_SED_Icon.ContainsKey(x.Key))
            {
                Destroy(dictionaly_SED_Icon[x.Key]);

                dictionaly_SED_Icon.Remove(x.Key);
            }

        }).AddTo(this);

        Base_TreasureData.onAct.Subscribe(x =>
        {
            treasureActImage.sprite = x.icon;

        }).AddTo(this);

        status.attack.abilityChargeValue.Subscribe(x =>
        {
            charaAbilityChargeValue.fillAmount = (float)x / (float)status.attack.charaAbility.requireChargeValue;

        }).AddTo(this);
    }
}
