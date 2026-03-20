using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Random = UnityEngine.Random;
using Cysharp.Threading.Tasks;
using System.Threading;

public class DamageTxtCtrler : MonoBehaviour
{
    [SerializeField] RectTransform rect;

    [SerializeField] TextMeshProUGUI txt;

    [SerializeField] float life = 0.75f;

    CancellationToken token;

    public void Initialize(int dmg, Color color, Vector2 pos)
    {
        rect.anchoredPosition = new Vector2(pos.x + Random.Range(-10f, 10f), pos.y + Random.Range(-10f, 10f));

        txt.color = color;
        txt.text = dmg.ToString();

        if(token == null) token = this.GetCancellationTokenOnDestroy();

        AnimationTask().Forget();
    }

    async UniTaskVoid AnimationTask()
    {
        this.gameObject.SetActive(true);

        transform.localScale = new Vector3(2.4f, 2.4f, 1f);

        await transform.DOScale(Vector3.one, 0.4f).ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, token);

        await UniTask.Delay((int)((life - 0.4f) * 1000), cancellationToken: token);

        await txt.DOFade(0, 0.25f).ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, token);

        this.gameObject.SetActive(false);
    }
}
