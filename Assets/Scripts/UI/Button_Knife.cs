using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UniRx;

public class Button_Knife : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image image_Sprite;

    [SerializeField] GameObject detailWindow;

    [SerializeField] Button button;

    Animator _animator;

    KnifeData_RunTime knifeData;
 
    //押されたときに飛ばす通知
    public Subject<KnifeData_RunTime> clicked { get; private set; } = new Subject<KnifeData_RunTime>();

    // カーソルが重なった、外れたら飛ばす通知
    public Subject<(KnifeData_RunTime knifeData, GameObject _object)> pointerEntered { get; private set; } = new();
    public Subject<(KnifeData_RunTime knifeData, GameObject _object)> pointerExited { get; private set; } = new ();

    void Start()
    {
        _animator = GetComponent<Animator>();

        button.onClick.AddListener(() => clicked.OnNext(knifeData));
    }

    // 初期化
    public void Initialize(KnifeData_RunTime x)
    {
        knifeData = x;

        image_Sprite.sprite = knifeData.sprite;

        //detailWindow.GetComponent<AKBtn_Detail>().Initialize(knifeData);

        //detailWindow.SetActive(false);
    }

    // カーソルが合ったとき
    public void OnPointerEnter(PointerEventData data)
    {
        // アニメーション
        _animator?.SetBool("highlighted", true);

        // 詳細ウインドウの表示
        //detailWindow.SetActive(true);

        // ヒエラルキーで最下に移動する
        // →前面に表示される
        //transform.SetAsLastSibling();

        pointerEntered.OnNext((knifeData, this.gameObject));
    }

    // カーソルが外れたとき
    public void OnPointerExit(PointerEventData data)
    {
        _animator?.SetBool("highlighted", false);

        //detailWindow.SetActive(false);

        pointerExited.OnNext((knifeData, this.gameObject));
    }

    private void OnDestroy()
    {
        clicked.Dispose();

        pointerEntered.Dispose();
        pointerExited.Dispose();
    }
}
