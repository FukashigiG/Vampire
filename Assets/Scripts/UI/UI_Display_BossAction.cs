using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class UI_Display_BossAction : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txt;

    Animator _animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animator = GetComponent<Animator>();

        EnemyCtrler_BigBoss.onAct.Subscribe(act =>
        {
            txt.text = act.actionName;

            _animator.SetTrigger("Trigger");

        }).AddTo(this);
    }
}
