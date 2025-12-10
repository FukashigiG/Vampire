using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using UnityEngine;

public class EP_Warning : MonoBehaviour
{
    [SerializeField] GameObject obj_Box;
    [SerializeField] GameObject obj_Circle;

    public async UniTask WarningAnim(float delayTime, CancellationToken token, AttackRangeType rangeType, float forwardDistance, float size_X = 0, float size_Y = 0, float size_Range = 0)
    {
        GameObject showObj = null;

        switch (rangeType)
        {
            case AttackRangeType.box:
                showObj = obj_Box;
                showObj.transform.localScale = new Vector3(size_X, size_Y, 1);
                break;

            case AttackRangeType.circle:
                showObj = obj_Circle;
                showObj.transform.localScale = new Vector3(size_Range * 2, size_Range * 2, 1);
                break;

            default:
                showObj = obj_Box;
                break;
        }

        showObj.transform.localPosition += Vector3.up * forwardDistance;

        showObj.SetActive(true);


        try
        {
            await showObj.transform.GetChild(0).transform.DOScale(Vector3.one, delayTime).ToUniTask(cancellationToken: token);

        }
        catch
        {
            throw;
        }

        Destroy(this.gameObject);
    }
}
