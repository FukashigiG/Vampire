using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

public class UI_FadeAnimator : MonoBehaviour
{
    [SerializeField] CanvasGroup _canvasGroup;

    async UniTask FadeOut()
    {
        _canvasGroup.alpha = 0;

        await _canvasGroup.DOFade(1, 1.5f).ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, this.GetCancellationTokenOnDestroy());
    }

    async UniTask FadeIn()
    {
        _canvasGroup.alpha = 1;

        await _canvasGroup.DOFade(0, 1.5f).ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, this.GetCancellationTokenOnDestroy());
    }
}
