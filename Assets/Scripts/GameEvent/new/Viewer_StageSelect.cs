using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class Viewer_StageSelect : SingletonMono<Viewer_StageSelect>
{
    [SerializeField] GameObject body_Panel;

    [SerializeField] Button[] buttons_Option;
    [SerializeField] Button button_Decide;

    [SerializeField] Text txt_StageName;

    List<StageData> stages = new List<StageData>();

    StageData currentSelected = null;

    private void Awake()
    {
        stages = Resources.LoadAll<StageData>("GameDatas/StageData").ToList();

        button_Decide.onClick.AddListener(Decision);
    }

    public void ShowEvent()
    {
        body_Panel.SetActive(true);

        // ボタンの数だけ、ランダムなステージを取得
        List<StageData> selected = Lottery_Stage(buttons_Option.Count());

        // それぞれのボタンに対して
        for (int i = 0; i < buttons_Option.Count(); i++)
        {
            buttons_Option[i].gameObject.SetActive(true);

            buttons_Option[i].onClick.RemoveAllListeners();

            var _stage = selected[i];

            buttons_Option[i].onClick.AddListener(() =>
            {
                ShowDiscription(_stage);
            });
        }

        currentSelected = null ;
    }

    void ShowDiscription(StageData data)
    {
        currentSelected = data ;

        txt_StageName.text = data.stageName;
    }

    void Decision()
    {
        if (currentSelected == null) return;

        GameAdmin.Instance.UpdateWave(currentSelected);

        ClosePanel();
    }

    void ClosePanel()
    {
        body_Panel.SetActive(false);
    }

    List<StageData> Lottery_Stage(int count)
    {
        // 全ステージのリストのコピーを取得
        List<StageData> candiData = new List<StageData>(stages);
        // 結果を返す箱も用意しておく
        List<StageData> result = new List<StageData>();

        // 現在のステージは抽選対象から外す
        if(candiData.Contains(GameAdmin.Instance.currentStage)) candiData.Remove(GameAdmin.Instance.currentStage);
 
        for(int i = 0; i < count; i++)
        {
            int targetRank = Lottery_StageRank(); // 1~3のランダムなランクを取得

            List<StageData> xxx = candiData
                .Where(x => x.stageRank == targetRank)// 抽選されたランクのやつを取得
                .OrderBy(x => UnityEngine.Random.value)// 順序シャッフル
                .ToList();

            // シャッフルされたリストの先頭を取得
            var add = xxx[0];

            // それをリザルトに追加
            result.Add(add);

            // 抽選の大本の箱から削除
            candiData.Remove(add);
        }

        return result;
    }

    // ステージのランクを抽選
    int Lottery_StageRank()
    {
        int[] rankWeights = { GetWeight(1) , GetWeight(2) , GetWeight(3) };

        int sum = 0;

        foreach(int x in rankWeights)
        {
            sum += x;
        }

        int randomPoint = Random.Range(1, sum + 1);

        int curent = 0;

        for (int i = 0; i < rankWeights.Length; i++)
        {
            curent += rankWeights[i];

            if (curent >= randomPoint)
            {
                return i+1;
            }
        }

        // 例外
        return 0;
    }

    // ランク抽選時の重さを取得
    int GetWeight(int stageRank)
    {
        // 基本的に低ランクほど抽選されやすくなるが、ウェーブカウントが進むほど、高ランクも出やすくなる

        int result =  (int)Mathf.Pow((4 - stageRank), 2) + GameAdmin.Instance.waveCount;

        return result;
    }
}
