using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;

public class PlayerPresenter : MonoBehaviour
{
    PlayerStatus status;

    [SerializeField] Image gauge_PlayerHP;
    [SerializeField] ShowPlayerHandState showHandState;

    // Start is called before the first frame update
    void Start()
    {
        status = GetComponent<PlayerStatus>();

        status.hitPoint.Subscribe(value =>
        {
            OnChangeHP_Value(value);

        }).AddTo(this);

        status.attack.onReload.Subscribe(list =>
        {
            var sprits = list.Select(knifeData => knifeData.sprite).ToList();

            showHandState.ShowReloadResult(sprits);

        }).AddTo(this);
    }

    void OnChangeHP_Value(int value)
    {
        gauge_PlayerHP.fillAmount = ((float)value / (float)status.maxHP);
    }
}
