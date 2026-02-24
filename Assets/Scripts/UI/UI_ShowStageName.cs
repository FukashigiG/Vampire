using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_ShowStageName : SingletonMono<UI_ShowStageName>
{
    [SerializeField] TextMeshProUGUI txt_Wave;
    [SerializeField] TextMeshProUGUI txt_StageName;

    [SerializeField] GameObject body;

    [SerializeField] Image[] icons_StageProgress;

    [SerializeField] Color color_Def;
    [SerializeField] Color color_HighLight;

    Animator _animator;

    protected override void Awake()
    {
        base.Awake();

        _animator = GetComponent<Animator>();
    }

    public void SetStageInfo(int waveCount, string stageName)
    {
        txt_Wave.text = "ウェーブ：" + waveCount;

        txt_StageName.text = stageName;

        foreach(var img in icons_StageProgress)
        {
            img.color = color_Def;
        }

        icons_StageProgress[waveCount - 1].color = color_HighLight;

        body.SetActive(true);

        _animator.SetTrigger("Show");
    }
}
