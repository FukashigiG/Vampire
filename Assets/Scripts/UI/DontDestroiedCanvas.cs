using UnityEngine;

public class DontDestroiedCanvas : SM_DontDestOnLand<DontDestroiedCanvas>
{
    protected override bool dontDestroyOnLoad { get { return true; } }
}
