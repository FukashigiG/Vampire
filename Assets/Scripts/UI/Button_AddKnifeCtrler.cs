using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UniRx;

public class Button_AddKnifeCtrler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image image_Sprite;

    [SerializeField] GameObject detailWindow;

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

        detailWindow.GetComponent<AKBtn_Detail>().Initialize(knifeData);

        detailWindow.SetActive(false);
    }

    // カーソルが合ったとき
    public void OnPointerEnter(PointerEventData data)
    {
        // アニメーション
        _animator?.SetBool("highlighted", true);

        // 詳細ウインドウの表示
        detailWindow.SetActive(true);

        // ヒエラルキーで最下に移動する
        // →前面に表示される
        transform.SetAsLastSibling();
    }

    // カーソルが外れたとき
    public void OnPointerExit(PointerEventData data)
    {
        _animator?.SetBool("highlighted", false);

        detailWindow.SetActive(false);
    }
}
