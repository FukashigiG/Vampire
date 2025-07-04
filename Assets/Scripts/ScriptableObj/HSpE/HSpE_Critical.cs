using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHSpE", menuName = "Game Data/HSpE/Critical")]
public class HSpE_Critical : BaseHSpE
{
    private bool _critical; // プロパティの値を保持するためのプライベートフィールド
    // これを下のcriticalに噛ませないと無限ループのエラーになる

    public override bool critical
    {
        get { return _critical; }
        set {  _critical = value; }
    }

    // 確率
    public float probability;

    public override void OnHitSpecialEffect(Base_MobStatus status, Vector2 posi)
    {
        float x = Random.Range(0f, 1f);

        if (x <= probability)
        {
            critical = true;
        }
        else
        {
            critical = false;
        }
    }
}
