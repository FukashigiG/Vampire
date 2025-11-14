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

    CancellationTokenSource tokenSource;

    public void SetGauge(float value)
    {
        if (tokenSource != null)
        {
            tokenSource.Cancel();
            tokenSource.Dispose();
        }

        tokenSource = new CancellationTokenSource();

        //Debug.Log(value);

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
