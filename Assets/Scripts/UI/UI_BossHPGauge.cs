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

        }).AddTo(bossObj);

        EnemyStatus.onDie
            .Where(x => x.status == status)
            .Subscribe(x =>
        {
            body.SetActive(false);

        }).AddTo(status);

        txt_BossName.text = status._enemyData._name;

        body.SetActive(true);
    }
}
