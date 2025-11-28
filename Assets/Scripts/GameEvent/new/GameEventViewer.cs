using UnityEngine;
using UnityEngine.UI;

public class GameEventViewer : SingletonMono<GameEventViewer>
{
    // ゲーム内イベント発動時、その内容を表示するUI用スクリプト

    [field:SerializeField] public GameObject body_Panel {  get; private set; }

    [SerializeField] Text titleText;
    [SerializeField] Text descriptionText;

    [SerializeField] Image illust;

    [SerializeField] RectTransform buttonArea;

    [SerializeField] GameObject prefab_OptionButton;

    public void ShowEvent(GameEventData _event)
    {
        body_Panel.SetActive(true);

        titleText.text = _event.eventTitle;
        descriptionText.text = _event.description;
        // illust.sprite = ...

        foreach(var option in _event.eventOptionLogic)
        {
            // ボタンエリアの子オブジェクトにボタン生成
            var button = Instantiate(prefab_OptionButton, buttonArea);

            // テキストをセット
            button.transform.GetChild(0).GetComponent<Text>().text = option.eventOptionText;

            // そのボタンが押されたら、
            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                option.Action();

                ClosePanel();
            });
        }
    }

    void ClosePanel()
    {
        //buttonAreaの子オブジェクトを全削除
        foreach (Transform button in buttonArea)
        {
            Destroy(button.gameObject);
        }

        body_Panel.SetActive(false);
    }
}
