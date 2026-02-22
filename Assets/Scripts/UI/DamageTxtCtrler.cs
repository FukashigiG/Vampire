using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Random = UnityEngine.Random;
using Cysharp.Threading.Tasks;

public class DamageTxtCtrler : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txt;

    [SerializeField] float life;

    public void Initialize(int dmg, Color color)
    {
        GetComponent<RectTransform>().anchoredPosition += new Vector2(Random.Range(-10f, 10f), Random.Range(-10f, 10f));

        txt.color = color;
        txt.text = dmg.ToString();

        AnimationTask().Forget();
    }

    async UniTask AnimationTask()
    {
        var token = this.GetCancellationTokenOnDestroy();

        transform.localScale = new Vector3(2.4f, 2.4f, 1f);

        await transform.DOScale(Vector3.one, 0.4f).ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, token);

        await UniTask.Delay((int)((life - 0.4f) * 1000), cancellationToken: token);

        await txt.DOFade(0, 0.25f).ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, token);

        Destroy(this.gameObject);
    }
}
