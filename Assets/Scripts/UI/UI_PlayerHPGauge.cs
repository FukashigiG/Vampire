using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_PlayerHPGauge : MonoBehaviour
{
    [SerializeField] Image gauge_PlayerHP;
    [SerializeField] Image hotzone_PlayerHP;
    [SerializeField] Text txt_PlayerHP;

    CancellationTokenSource tokenSource;

    void Start()
    {
        
    }

    public void SetGauge(int hp, int maxHp)
    {
        float fill = (float)hp / (float)maxHp;

        gauge_PlayerHP.fillAmount = fill;

        txt_PlayerHP.text = $"HP : {hp} / {maxHp}";

        if (tokenSource != null)
        {
            tokenSource.Cancel();
            tokenSource.Dispose();
        }

        tokenSource = new CancellationTokenSource();

        HotzoneAnim(fill , tokenSource.Token).Forget();
    }

    async UniTask HotzoneAnim(float value , CancellationToken token)
    {
        await UniTask.Delay((int)(1000 * 1.5f), cancellationToken: token);

        await hotzone_PlayerHP.DOFillAmount(value, 0.4f).ToUniTask(cancellationToken: token);
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
