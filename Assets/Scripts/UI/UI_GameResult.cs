using UnityEngine;
using UnityEngine.UI;

public class UI_GameResult : SingletonMono<UI_GameResult>
{
    [SerializeField] GameObject body;

    [SerializeField] Text txt_Result;

    [SerializeField] Animator _animator;

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

        body.SetActive(true);
    }
}
