using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UniRx;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] KnifeData defKnife;

    [SerializeField] float coolTime_ThrowKnife;
    [SerializeField] float time_ReloadKnives;

    [SerializeField] LayerMask targetLayer;

    PlayerStatus status;

    GameObject targetEnemy;

    List<KnifeData> availableKnifes = new List<KnifeData>();

    // �i�C�t�������O�ɔ��s�A�����ʂŕҏW�ł���悤��
    public Subject<KnifeData> onThrowKnife { get; private set; } = new Subject<KnifeData>();

    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
    CancellationToken _token;

    private void Awake()
    {
        status = GetComponent<PlayerStatus>();

        _token = cancellationTokenSource.Token;
    }

    void Start()
    {
        AttackTask(_token).Forget();
    }

    private void Update()
    {
        targetEnemy = FindEnemy();
    }

    // �U���T�C�N������
    async UniTask AttackTask(CancellationToken token)
    {
        while (true)
        {
            await Reload();

            await ThrowKnives(_token);
        }
    }

    // �U���Ώۂ̒T��
    GameObject FindEnemy()
    {

        //���͈͓��̓G��z��Ɋi�[
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, status.eyeSight, targetLayer);

        GameObject nearestObject = null;
        float shortestDistance = Mathf.Infinity; // ������ŏ�����

        // ��ԋ߂��G��T��
        foreach (Collider2D hit in hits)
        {
            float Distance = Vector2.Distance(transform.position, hit.transform.position);

            if(Distance < shortestDistance )
            {
                shortestDistance = Distance;
                nearestObject = hit.gameObject;
            }
        }

        return nearestObject;
    }

    async UniTask Reload()
    {
        availableKnifes = status.inventory.runtimeKnives;

        await UniTask.Delay((int)(time_ReloadKnives * 1000));

        Debug.Log("reload : " + availableKnifes.Count);
    }

    async UniTask ThrowKnives(CancellationToken token)
    {
        for (int i = 0; i < availableKnifes.Count; i++)
        {
            // �U���͈͓��ɓG�������܂ő҂�
            await UniTask.WaitUntil(() => targetEnemy != null, cancellationToken: token);

            // �U���Ώۂ̕�����Vec2�^�Ŏ擾
            Vector2 dir = (targetEnemy.transform.position - this.transform.position).normalized;

            // �G�f�B�^��œo�^���ꂽScriptableObject���擾
            var originalKnifeData = availableKnifes[i];

            // ScriptableObject��Instantiate���āA���s����p�̃R�s�[�𐶐�����
            // �������邱�ƂŁA���ۂ�ScriptableObject�̓��e�����������邱�ƂȂ��A�ҏW���ꂽKnifeData���������Ƃ��ł���
            var knife = Instantiate(originalKnifeData);

            // �w�ǐ�ɂ�����̂��߂̔��s
            onThrowKnife.OnNext(knife);

            // �i�C�t�𐶐��A�����x�ƒu��
            // �ҏW���ꂽ�\���̂���KnifeData�ŏ����𑱍s
            var x = Instantiate(knife.prefab, this.transform.position, Quaternion.FromToRotation(Vector2.up, dir));

            // x��������
            x.GetComponent<Base_KnifeCtrler>().Initialize(status.throwPower, knife);

            await UniTask.Delay((int)(coolTime_ThrowKnife * 1000), cancellationToken: token);
        }
    }

    private void OnDestroy()
    {
        cancellationTokenSource.Cancel();
        cancellationTokenSource.Dispose();

        onThrowKnife.Dispose();
    }
}
