using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_GameResult : SingletonMono<UI_GameResult>
{
    [SerializeField] GameObject body;

    [SerializeField] TextMeshProUGUI txt_Result;
    [SerializeField] TextMeshProUGUI txt_WaveCount;

    [SerializeField] Animator _animator;

    [SerializeField] Image[] _image;

    [SerializeField] Sprite sprite_Icon_Null;

    [SerializeField] Button button_GoBack;

    public void OnGameSet(bool isPlayerWin)
    {
        switch(isPlayerWin)
        {
            case true:
                txt_Result.text = "勝利";
                break;

            case false:
                txt_Result.text = "敗北...";
                break;
        }

        txt_WaveCount.text = "進んだウェーブ数：" + GameAdmin.Instance.waveCount;

        for(int i = 0; i < _image.Length; i++)
        {
            if (GameAdmin.Instance.stageHistory.Count >= i + 1)
            {
                _image[i].sprite = GameAdmin.Instance.stageHistory[i].groungSprite;
            }
            else
            {
                _image[i].sprite = sprite_Icon_Null;
            }
        }

        body.SetActive(true);

        _animator.SetTrigger("Trigger");

        button_GoBack.onClick.RemoveAllListeners();
        button_GoBack.onClick.AddListener(() =>
        {
            GameAdmin.Instance.ReTry();
        });
    }
}
