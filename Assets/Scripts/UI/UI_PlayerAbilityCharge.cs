using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerAbilityCharge : MonoBehaviour
{
    // キャラアビリティゲージ管理用スクリプト

    [SerializeField] Image gauge;

    Animator animator;

    CancellationTokenSource tokenSource;

    // 外部からInitializeさせる方が良い
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetGauge(float value)
    {
        if (tokenSource != null)
        {
            tokenSource.Cancel();
            tokenSource.Dispose();
        }

        if (value >= 1f)
        {
            animator.SetTrigger("Anim");
        }

        tokenSource = new CancellationTokenSource();

        AnimGauge(value, tokenSource.Token).Forget();
    }

    async UniTask AnimGauge(float value, CancellationToken token)
    {
        await gauge.DOFillAmount(value, 0.4f).ToUniTask(cancellationToken: token);
    }

    private void OnDestroy()
    {
        if (tokenSource != null)
        {
            tokenSource.Cancel();
            tokenSource.Dispose();
        }
    }
}
