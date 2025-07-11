using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHSpE", menuName = "Game Data/HSpE/penetration")]
public class HSpE_Penetration : BaseHSpE
{
    // dontDestroyBulletをtrueに上書きすることで貫通を許可
    public override bool dontDestroyBullet
    {
        get { return true; }
    }
}
