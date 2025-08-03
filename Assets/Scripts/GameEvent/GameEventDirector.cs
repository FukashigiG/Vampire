using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameEventDirector : SingletonMono<GameEventDirector>
{
    [SerializeField] GameObject panel_GetKnife;
    [SerializeField] GameObject panel_GetTreasure;

    CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    CancellationToken _cancellationToken;

    private void Awake()
    {
        _cancellationToken = _cancellationTokenSource.Token;
    }

    private void Start()
    {
        panel_GetKnife.GetComponent<Eve_GetKnife>().Initialize();
        panel_GetTreasure.GetComponent<Eve_GetTreasure>().Initialize();
    }

    public enum Events
    {
        getKnife,
        getTreasure
    }

    public void TriggerEvent(Events _event)
    {
        switch( _event )
        {
            case Events.getKnife:
                EventAsync(panel_GetKnife, _cancellationToken).Forget();
                break;

            case Events.getTreasure:
                EventAsync(panel_GetTreasure, _cancellationToken).Forget();
                break;
        }
    }

    async UniTask EventAsync(GameObject panel, CancellationToken token)
    {
        // �Q�[���̈ꎞ��~
        GameAdmin.Instance.PauseGame();

        // �����œn���ꂽ�p�l�����J��
        panel.SetActive(true);

        // �p�l��������܂ő҂�
        await UniTask.WaitUntil(() => panel.activeSelf == false, cancellationToken: token);

        // �Q�[���̍ĊJ
        GameAdmin.Instance.ResumeGame();
    }

    private void OnDestroy()
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
    }
}
