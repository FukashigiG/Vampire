using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLvUpManager : MonoBehaviour
{
    [SerializeField] GameObject button_Option;
    [SerializeField] GameObject buttonArea;

    [SerializeField] List<KnifeData> allKnifeData;

    private void OnEnable()
    {
        int num_Option = Random.Range(2, 6);

        for (int i = 0; i < num_Option; i++)
        {
            //���������{�^����x�ƒu��
            var x =  Instantiate(button_Option, buttonArea.transform);

            var y = DrawingKnives();

            //x��button�R���|�[�l���g��onClick�ɔ�������Choice�֐������s����
            x.GetComponent<Button>().onClick.AddListener(() => Choice(y));
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
