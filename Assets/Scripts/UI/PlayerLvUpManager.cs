using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class PlayerLvUpManager : MonoBehaviour
{
    [SerializeField] GameObject button_Option;
    [SerializeField] GameObject buttonArea;

    [SerializeField] List<KnifeData> allKnifeData;

    [SerializeField] Button button_Skip;

    private void Start()
    {
        //�i�C�t�ǉ���ʂ̃{�^���������ꂽ�ۂɁA��������m���֐������s
        Button_Knife.clicked.Subscribe(xx => Choice(xx)).AddTo(this);

        // �X�L�b�v�{�^�����������̂ɔ������āA�p�l�������悤��
        button_Skip.onClick.AddListener(() => this.gameObject.SetActive(false));
    }


    //�p�l����Active�ɂȂ����Ƃ�
    private void OnEnable()
    {
        int num_Option = Random.Range(2, 6);

        //�Q�`�T�̃{�^����p��
        for (int i = 0; i < num_Option; i++)
        {
            //���������{�^����buttonObj�ƒu��
            var buttonObj =  Instantiate(button_Option, buttonArea.transform);

            //�����_���ȃi�C�t��I�o
            var randomKnife = DrawingKnives();

            //�R���|�[�l���g�̎擾
            var buttonCtrler = buttonObj.GetComponent<Button_Knife>();

            //�{�^���ɑI�o�����i�C�t�̏���n��
            buttonCtrler.Initialize(randomKnife);

            //�{�^���������ꂽ�ۂ̏�����Start()�ɂċL�ڍς�

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

    void Choice(KnifeData knifeData)
    {
        //�v���C���[�ɒ��I���ꂽ�i�C�t�̒ǉ�
        PlayerController.Instance._status.inventory.AddKnife(knifeData);

        //�p�l�������
        this.gameObject.SetActive(false);
    }

    //�i�C�t�̒��I
    KnifeData DrawingKnives()
    {
        int x = Random.Range(0, allKnifeData.Count);

        var y = allKnifeData[x];

        return y;
    }

    // ���̃p�l��������Ƃ��iactiveSelf��false�ɂȂ�Ƃ��j
    private void OnDisable()
    {
        //buttonArea�̎q�I�u�W�F�N�g��S�폜
        foreach (Transform button in buttonArea.transform)
        {
            Destroy(button.gameObject);
        }
    }
}
