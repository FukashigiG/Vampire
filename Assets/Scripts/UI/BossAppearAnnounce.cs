using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;
using TMPro;

public class BossAppearAnnounce : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txt;

    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();

        GameAdmin.Instance.onBossAppear.Subscribe(x =>
        {
            if(x == true)
            {
                txt.color = Color.red;
                txt.text = "強敵乱入！!";
            }
            else
            {
                txt.color = Color.orange;
                txt.text = "ボス出現！";
            }

            animator.SetTrigger("Anim");

        }).AddTo(this);
    }
}
