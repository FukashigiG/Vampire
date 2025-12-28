using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;

public class UI_BossHPGauge : SingletonMono<UI_BossHPGauge>
{
    [SerializeField] GameObject body;

    [SerializeField] Text txt_BossName;

    [SerializeField] Image gauge;

    public void Initialize(GameObject bossObj)
    {
        gauge.fillAmount = 1f;

        var status = bossObj.GetComponent<EnemyStatus>();

        status.hitPoint.Subscribe(x =>
        {
            gauge.fillAmount = (float)x / status.maxHP;

        }).AddTo(bossObj);

        status.onDie.Subscribe(x =>
        {
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
