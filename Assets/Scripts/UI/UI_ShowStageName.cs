using UnityEngine;
using UnityEngine.UI;

public class UI_ShowStageName : SingletonMono<UI_ShowStageName>
{
    [SerializeField] Text txt_Wave;
    [SerializeField] Text txt_StageName;

    [SerializeField] GameObject body;

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

        body.SetActive(true);

        _animator.SetTrigger("Show");
    }
}
