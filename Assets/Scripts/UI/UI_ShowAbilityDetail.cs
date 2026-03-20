using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UI_ShowAbilityDetail : SingletonMono<UI_ShowAbilityDetail>
{
    List<Window_ShowDetail> windows = new List<Window_ShowDetail>();

    protected override void Awake()
    {
        foreach(Transform child in this.transform)
        {
            if(child.gameObject.TryGetComponent(out Window_ShowDetail window))
            {
                windows.Add(window);
            }
        }
    }

    public void Show(IDiscribing discribing, int order = 0)
    {
        if(discribing._name == string.Empty || discribing._name == "" || discribing._name == null) return;
        if(order >= windows.Count) return;

        windows[order].OpenWindow(discribing);

        if(discribing.ex_Discribing != null)
        {
            Show(discribing.ex_Discribing, order + 1);
        }
    }

    public void Hide()
    {
        foreach(Window_ShowDetail window in windows)
        {
            window.gameObject.SetActive(false);
        }
    }
}
