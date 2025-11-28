using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameEventDirector : SingletonMono<GameEventDirector>
{
    /*[SerializeField] GameObject panel_GetKnife;
    [SerializeField] GameObject panel_GetTreasure;
    [SerializeField] GameObject panel_DriveKnife;*/

    CancellationToken _token;

    private void Awake()
    {
        _token = this.GetCancellationTokenOnDestroy();
    }

    private void Start()
    {
        /*panel_GetKnife.GetComponent<Eve_GetKnife>().Initialize();
        panel_GetTreasure.GetComponent<Eve_GetTreasure>().Initialize();
        panel_DriveKnife.GetComponent<Eve_DriveKnife>().Initialize();*/
    }

    public enum Events
    {
        getKnife,
        getTreasure,
        fusionKnives,
        driveKnife
    }

    public void TriggerEvent(GameEventData eventData)
    {
        EventAsync(eventData, _token).Forget();
    }

    async UniTask EventAsync(GameEventData eventData, CancellationToken token)
    {
        // ゲームの一時停止
        GameAdmin.Instance.PauseGame();

        // パネルを開く
        GameEventViewer.Instance.ShowEvent(eventData);

        GameObject panel = GameEventViewer.Instance.gameObject;

        // パネルが閉じるまで待つ
        await UniTask.WaitUntil(() => panel.activeSelf == false, cancellationToken: token);

        // ゲームの再開
        GameAdmin.Instance.ResumeGame();
    }
}
