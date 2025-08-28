using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Base_EventCtrler : MonoBehaviour
{
    [SerializeField] Image EventSpriteArea;
    [SerializeField] protected Transform buttonArea;

    [SerializeField] protected Button button_Skip;

    public virtual void Initialize()
    {
        // スキップボタンが押されるのに反応して、パネルを閉じるように
        button_Skip?.onClick.AddListener(() => this.gameObject.SetActive(false));
    }

    protected virtual void OnEnable()
    {

    }

    protected void DisposeButtons()
    {
        //buttonAreaの子オブジェクトを全削除
        foreach (Transform button in buttonArea)
        {
            Destroy(button.gameObject);
        }
    }

    // このパネルが閉じるとき（activeSelfがfalseになるとき）
    protected void OnDisable()
    {
        DisposeButtons();
    }
}
