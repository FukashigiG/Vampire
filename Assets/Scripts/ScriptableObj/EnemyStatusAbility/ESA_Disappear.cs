using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewESA", menuName = "Game Data/EnemyStatusAbilityData/Disappear")]
public class ESA_Disappear : Base_EnemyStatusAbilityData
{
    // Nïbå„Ç…é©ìÆìIÇ…è¡ñ≈Ç∑ÇÈ

    [SerializeField] float lifeTime;

    public override void ApplyAbility(EnemyStatus status, CompositeDisposable disposables)
    {
        WaitAndDissapear(status.gameObject).Forget();
    }

    async UniTask WaitAndDissapear(GameObject body)
    {
        var token = body.GetCancellationTokenOnDestroy();

        await UniTask.Delay((int)(lifeTime * 1000), cancellationToken: token);

        token.ThrowIfCancellationRequested();

        if (body == null) return;

        Destroy(body);
    }
}
