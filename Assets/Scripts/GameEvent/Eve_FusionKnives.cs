using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Eve_FusionKnives : Base_EventCtrler
{
    PlayerInventory inventory;

    [SerializeField] GameObject button_Knife;

    public override void Initialize()
    {
        base.Initialize();

        inventory = PlayerController.Instance._status.inventory;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        // �莝���i�C�t�S�Ẵ{�^���𐶐�
        foreach(var knife in inventory.runtimeKnives)
        {
            // �{�^���𐶐�
            GameObject button = Instantiate(button_Knife, buttonArea);

            // �{�^���Ƀi�C�t�̏���n���ď�����
            button.GetComponent<Button_Knife>().Initialize(knife);

            // �{�^���������ꂽ�Ƃ��̏�����o�^
            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                Clicked(knife);
            });
        }
    }

    // �{�^���������ꂽ���̏���
    void Clicked(KnifeData knifeData)
    {

    }
}
