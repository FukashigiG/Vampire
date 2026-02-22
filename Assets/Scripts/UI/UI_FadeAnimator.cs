using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

public class UI_FadeAnimator : MonoBehaviour
{
    [SerializeField] GameObject body;

    [SerializeField] CanvasGroup _canvasGroup;

    [SerializeField] Animator _animator;

    public async UniTask FadeOut()
    {
        body.SetActive(true);

        var token = this.GetCancellationTokenOnDestroy();

        //_canvasGroup.alpha = 0;

        //await _canvasGroup.DOFade(1, 1.5f).ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, this.GetCancellationTokenOnDestroy());

        _animator.SetTrigger("FadeOut");

        // アニメーション偏移
        await UniTask.Yield(PlayerLoopTiming.Update, token);

        // アニメーションが終わるまで待機
        await UniTask.WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);

    }

    public async UniTask FadeIn()
    {
        // _canvasGroup.alpha = 1;

        // await _canvasGroup.DOFade(0, 1.5f).ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, this.GetCancellationTokenOnDestroy());

        var token = this.GetCancellationTokenOnDestroy();

        _animator.SetTrigger("FadeIn");

        // アニメーション偏移
        await UniTask.Yield(PlayerLoopTiming.Update, token);

        Debug.Log(_animator.GetCurrentAnimatorStateInfo(0).IsName("FadeIn"));

        // アニメーションが終わるまで待機
        await UniTask.WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
    }
}
