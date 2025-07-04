using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHSpE", menuName = "Game Data/HSpE/IgnoreDefence")]
public class HSpE_IgnoreDefence : BaseHSpE
{
    // IgnoreDefenceをtrueに上書きすることで防御無視を許可
    public override bool ignoreDefence
    {
        get { return true; } 
    }
}
