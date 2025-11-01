using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class Eve_GetKnife : Base_EventCtrler
{
    [SerializeField] GameObject button_Option;

    [SerializeField] Button button_reroll;

    [SerializeField] List<KnifeData> allKnifeData;

    public override void Initialize()
    {
        base.Initialize();

        button_reroll.onClick.AddListener(() =>
        {
            Reroll();
        });

        
    }

    //�p�l����Active�ɂȂ����Ƃ�
    protected override void OnEnable()
    {
        base.OnEnable();

        GenerateButtons();
    }

    void GenerateButtons()
    {
        int num_Option = Random.Range(2, 6);

        //�Q�`�T�̃{�^����p��
        for (int i = 0; i < num_Option; i++)
        {
            //�����_���ȃi�C�t��I�o
            var randomKnife = DrawingKnives();

            //���������{�^����buttonObj�ƒu��
            var buttonObj = Instantiate(button_Option, buttonArea);

            //�R���|�[�l���g�̎擾
            var buttonCtrler = buttonObj.GetComponent<Button_Knife>();

            //�{�^���ɑI�o�����i�C�t�̏���n��
            buttonCtrler.Initialize(randomKnife);

            //�{�^���������ꂽ�ۂɁA��������m���֐������s
            buttonCtrler.clicked.Subscribe(xx => Choice(xx)).AddTo(buttonCtrler);

            // �ʒu��ύX

            float x;

            switch (num_Option)
            {
                case 2:
                    x = 500;
                    break;

                case 3:
                    x = 650;
                    break;

                default:
                    x = 800;
                    break;
            }

            float y = x / (num_Option - 1);

            buttonObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(y * i - x / 2, 0);
        }
    }

    void Choice(KnifeData_RunTime knifeData)
    {
        //�v���C���[�ɒ��I���ꂽ�i�C�t�̒ǉ�
        PlayerController.Instance._status.inventory.AddKnife(knifeData);

        //�p�l�������
        this.gameObject.SetActive(false);
    }

    //�i�C�t�̒��I
    KnifeData_RunTime DrawingKnives()
    {
        int x = Random.Range(0, allKnifeData.Count);

        var y = new KnifeData_RunTime(allKnifeData[x]);

        return y;
    }

    void Reroll()
    {
        DisposeButtons();

        GenerateButtons();
    }
}
