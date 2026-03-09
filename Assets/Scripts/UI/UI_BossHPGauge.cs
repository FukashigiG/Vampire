using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;
using TMPro;
using System.Collections.Generic;

public class UI_BossHPGauge : SingletonMono<UI_BossHPGauge>
{
    [SerializeField] GameObject iconPrefab;

    [SerializeField] GameObject body;

    [SerializeField] Transform iconArea;

    [SerializeField] TextMeshProUGUI txt_BossName;

    [SerializeField] Image gauge;

    Dictionary<Base_StatusEffectData, GameObject> dictionaly_SED_Icon = new();

    public void Initialize(GameObject bossObj)
    {
        gauge.fillAmount = 1f;

        var status = bossObj.GetComponent<EnemyStatus>();

        // HP増減を購読
        status.hitPoint.Subscribe(x =>
        {
            gauge.fillAmount = (float)x / status.maxHP;

        }).AddTo(bossObj);

        // 状態変化を購読、アイコンを生成・表示
        status.activeStatusTypeCounts.ObserveAdd().Subscribe(x =>
        {
            GameObject obj = Instantiate(iconPrefab, iconArea);

            obj.GetComponent<Image>().sprite = x.Key.icon;

            dictionaly_SED_Icon.Add(x.Key, obj);

        }).AddTo(bossObj);

        // 状態変化終了を購読、アイコンを破棄
        status.activeStatusTypeCounts.ObserveRemove().Subscribe(x =>
        {
            if (dictionaly_SED_Icon.ContainsKey(x.Key))
            {
                Destroy(dictionaly_SED_Icon[x.Key]);

                dictionaly_SED_Icon.Remove(x.Key);
            }

        }).AddTo(bossObj);

        status.onDie.Subscribe(x =>
        {
            foreach(var dex in dictionaly_SED_Icon)
            {
                Destroy(dex.Value);
            }

            dictionaly_SED_Icon.Clear();

            OnDisappear().Forget();

        }).AddTo(status);

        txt_BossName.text = status._enemyData._name;

        OnAppearAnim().Forget();
    }

    async UniTaskVoid OnAppearAnim()
    {
        GetComponent<CanvasGroup>().alpha = 1f;

        body.SetActive(true);

        var rect = body.GetComponent<RectTransform>();

        rect.anchoredPosition += Vector2.up * 100f;

        gauge.fillAmount = 0f;

        var moveTask = rect.DOAnchorPosY(0f, 1f).ToUniTask(TweenCancelBehaviour.KillAndCancelAwait);

        var fillTask = gauge.DOFillAmount(1f, 1f).ToUniTask(TweenCancelBehaviour.KillAndCancelAwait);

        await UniTask.WhenAll(moveTask, fillTask);
    }

    async UniTaskVoid OnDisappear()
    {
        await GetComponent<CanvasGroup>().DOFade(0, 1f).ToUniTask(TweenCancelBehaviour.KillAndCancelAwait);

        body.SetActive(false);
    }
}
