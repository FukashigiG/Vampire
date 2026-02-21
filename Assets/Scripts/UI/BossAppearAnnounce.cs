using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;

public class BossAppearAnnounce : MonoBehaviour
{
    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();

        GameAdmin.Instance.onBossAppear.Subscribe(x =>
        {
            animator.SetTrigger("Anim");

        }).AddTo(this);
    }
}
