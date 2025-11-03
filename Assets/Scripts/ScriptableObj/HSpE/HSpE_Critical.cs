using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHSpE", menuName = "Game Data/HSpE/Critical")]
public class HSpE_Critical : BaseHSpE
{
    // まれに大きなダメージ

    private bool _critical = false; // プロパティの値を保持するためのプライベートフィールド
    // これを下のcriticalに噛ませないと無限ループのエラーになる

    public override bool critical
    {
        get { return _critical; }
    }

    [SerializeField] GameObject efect;

    // 効果が発動して初めてtrueになる
    protected override void ActivateEffect(Base_MobStatus status, Vector2 posi, KnifeData_RunTime knifeData)
    {
        _critical = true;

        Instantiate(efect, posi, Quaternion.identity);

        base.ActivateEffect(status, posi, knifeData);
    }
}
