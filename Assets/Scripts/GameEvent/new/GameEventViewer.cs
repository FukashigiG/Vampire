using UnityEngine;
using UnityEngine.UI;

public class GameEventViewer : SingletonMono<GameEventViewer>
{
    [SerializeField] Text titleText;
    [SerializeField] Text descriptionText;

    [SerializeField] Image illust;

    [SerializeField] RectTransform buttonArea;

    [SerializeField] GameObject prefab_OptionButton;

    public void ShowEvent(GameEventData _event)
    {
        this.gameObject.SetActive(true);

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

                this.gameObject.SetActive(false);
            });
        }
    }

    void DisposeButtons()
    {
        //buttonAreaの子オブジェクトを全削除
        foreach (Transform button in buttonArea)
        {
            Destroy(button.gameObject);
        }
    }

    // このパネルが閉じるとき（activeSelfがfalseになるとき）
    void OnDisable()
    {
        DisposeButtons();
    }
}
