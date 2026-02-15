using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

public class UI_FadeAnimator : MonoBehaviour
{
    [SerializeField] GameObject body;

    [SerializeField] CanvasGroup _canvasGroup;

    public async UniTask FadeOut()
    {
        body.SetActive(true);

        _canvasGroup.alpha = 0;

        await _canvasGroup.DOFade(1, 1.5f).ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, this.GetCancellationTokenOnDestroy());
    }

    public async UniTask FadeIn()
    {
        _canvasGroup.alpha = 1;

        await _canvasGroup.DOFade(0, 1.5f).ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, this.GetCancellationTokenOnDestroy());
    }
}
