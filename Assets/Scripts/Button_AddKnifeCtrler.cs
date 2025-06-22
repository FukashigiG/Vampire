using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UniRx;

public class Button_AddKnifeCtrler : MonoBehaviour
{
    [SerializeField] Image image_Sprite;

    [SerializeField] Button button;

    Animator _animator;

    KnifeData knifeData;

    //押されたときに飛ばす通知
    //ゲーム終了時にはGameAdminにDisposeされる
    public static Subject<KnifeData> clicked { get; private set; } = new Subject<KnifeData>();

    void Start()
    {
        _animator = GetComponent<Animator>();

        button.onClick.AddListener(() => clicked.OnNext(knifeData));
    }

    // 初期化
    public void Initialize(KnifeData x)
    {
        knifeData = x;

        image_Sprite.sprite = knifeData.sprite;
    }

    // カーソルが合ったとき
    public void OnPointerEnter(PointerEventData data)
    {
        _animator?.SetBool("highlighted", true);

        Debug.Log("dfd");
    }

    // カーソルが外れたとき
    public void OnPointerExit(PointerEventData data)
    {
        _animator?.SetBool("highlighted", true);
    }
}
