using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using UnityEngine;

public class StageGroundCtrler : SingletonMono<StageGroundCtrler>
{
    [SerializeField] SpriteRenderer groundSR;
    [SerializeField] SpriteRenderer faderSR;

    public void ChangeGroundImg(Sprite sprite)
    {
        ChangeTask(sprite).Forget();
    }

    async UniTaskVoid ChangeTask(Sprite _sprite)
    {
        CancellationToken token = this.gameObject.GetCancellationTokenOnDestroy();

        await faderSR.DOColor(new Color(0, 0, 0, 1f), 0.6f).ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, cancellationToken: token);

        groundSR.sprite = _sprite;

        await faderSR.DOColor(new Color(0, 0, 0, 0f), 0.6f).ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, cancellationToken: token);
    }
}
