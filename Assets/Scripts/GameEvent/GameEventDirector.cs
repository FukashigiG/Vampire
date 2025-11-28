using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameEventDirector : SingletonMono<GameEventDirector>
{
    CancellationToken _token;

    private void Awake()
    {
        _token = this.GetCancellationTokenOnDestroy();
    }

    private void Start()
    {

    }

    enum EventsType { getSome, _event }

    public void Trigger_GetSome()
    {
        EventAsync(EventsType.getSome, _token).Forget();
    }

    public void Trigger_Event(GameEventData eventData)
    {
        EventAsync(EventsType._event , _token, eventData).Forget();
    }

    async UniTask EventAsync(EventsType type, CancellationToken token, GameEventData eventData = null)
    {
        // ゲームの一時停止
        GameAdmin.Instance.PauseGame();

        GameObject panel = null;

        switch (type)
        {
            case EventsType.getSome:

                // パネルを開く
                GetSomeoneViewer.Instance.ShowEvent();

                panel = GetSomeoneViewer.Instance.body_Panel;

                break;

            case EventsType._event:

                // パネルを開く
                GameEventViewer.Instance.ShowEvent(eventData);

                panel = GameEventViewer.Instance.body_Panel;

                break;
        }



        // パネルが閉じるまで待つ
        await UniTask.WaitUntil(() => panel.activeSelf == false, cancellationToken: token);

        // ゲームの再開
        GameAdmin.Instance.ResumeGame();
    }
}
