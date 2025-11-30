using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Button_Treasure : MonoBehaviour
{
    [SerializeField] Image image_Sprite;

    //[SerializeField] GameObject detailWindow;

    [SerializeField] Text text_Name_Kari;
    [SerializeField] Text text_Description_Kari;

    [SerializeField] Button button;

    Animator _animator;

    TreasureData treasureData;

    //押されたときに飛ばす通知
    //ゲーム終了時にはGameAdminにDisposeされる
    public static Subject<TreasureData> clicked { get; private set; } = new Subject<TreasureData>();

    // カーソルが重なった、外れたら飛ばす通知
    public Subject<TreasureData> pointerEntered { get; private set; } = new Subject<TreasureData>();
    public Subject<TreasureData> pointerExited { get; private set; } = new Subject<TreasureData>();

    void Start()
    {
        _animator = GetComponent<Animator>();

        button.onClick.AddListener(() => clicked.OnNext(treasureData));
    }

    // 初期化
    public void Initialize(TreasureData x)
    {
        treasureData = x;

        image_Sprite.sprite = treasureData.sprite;

        text_Name_Kari.text = treasureData._name;
        text_Description_Kari.text = treasureData.flavortext;

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

        pointerEntered.OnNext(treasureData);
    }

    // カーソルが外れたとき
    public void OnPointerExit(PointerEventData data)
    {
        _animator?.SetBool("highlighted", false);

        //detailWindow.SetActive(false);

        pointerExited.OnNext(treasureData);
    }

    private void OnDestroy()
    {
        pointerEntered.Dispose();
        pointerExited.Dispose();
    }
}
