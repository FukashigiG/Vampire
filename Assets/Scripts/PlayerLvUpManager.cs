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

    private void Start()
    {
        //�i�C�t�ǉ���ʂ̃{�^���������ꂽ�ۂɁA��������m���֐������s
        Button_AddKnifeCtrler.clicked.Subscribe(xx => Choice(xx));
    }


    //�p�l����Active�ɂȂ����Ƃ�
    private void OnEnable()
    {
        int num_Option = Random.Range(2, 6);

        //�Q�`�T�̃{�^����p��
        for (int i = 0; i < num_Option; i++)
        {
            //���������{�^����x�ƒu��
            var buttonObj =  Instantiate(button_Option, buttonArea.transform);

            //�����_���ȃi�C�t��I�o
            var randomKnife = DrawingKnives();

            //�R���|�[�l���g�̎擾
            var buttonCtrler = buttonObj.GetComponent<Button_AddKnifeCtrler>();

            //�{�^���ɑI�o�����i�C�t�̏���n��
            buttonCtrler.SetInfo(randomKnife);

            //�{�^���������ꂽ�ۂ̏�����Start()�ɂċL�ڍς�
        }
    }

    void Choice(KnifeData knifeData)
    {
        //�v���C���[�ɒ��I���ꂽ�i�C�t�̒ǉ�
        //���΂Ƀ��P�N�\����������̂ŏC���K�{
        GameObject.Find("Player").GetComponent<PlayerAttack>().AddKnife(knifeData);

        //buttonArea�̎q�I�u�W�F�N�g��S�폜
        foreach (Transform button in buttonArea.transform)
        {
            Destroy(button.gameObject);
        }

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
}
