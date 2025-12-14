using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class UI_BossHPGauge : SingletonMono<UI_BossHPGauge>
{
    [SerializeField] GameObject body;

    [SerializeField] Text txt_BossName;

    [SerializeField] Image gauge;

    public void Initialize(GameObject bossObj)
    {
        gauge.fillAmount = 1f;

        var status = bossObj.GetComponent<EnemyStatus>();

        status.hitPoint.Subscribe(x =>
        {
            gauge.fillAmount = (float)x / status.maxHP;

            Debug.Log((float)x / status.maxHP);

        }).AddTo(bossObj);

        txt_BossName.text = status._enemyData._name;

        body.SetActive(true);
    }
}
