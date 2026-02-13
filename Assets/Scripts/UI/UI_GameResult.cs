using UnityEngine;
using UnityEngine.UI;

public class UI_GameResult : SingletonMono<UI_GameResult>
{
    [SerializeField] GameObject body;

    [SerializeField] Text txt_Result;

    [SerializeField] Animator _animator;

    [SerializeField] Image[] _image;

    [SerializeField] Sprite sprite_Icon_Null;

    [SerializeField] Button button_GoBack;

    public void OnGameSet(bool isPlayerWin)
    {
        switch(isPlayerWin)
        {
            case true:
                txt_Result.text = "èüóò";
                break;

            case false:
                txt_Result.text = "îsñk...";
                break;
        }

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
