using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eve_GetTreasure : Base_EventCtrler
{
    [SerializeField] GameObject treasureButtonObj;

    [SerializeField] Transform buttonArea;

    [SerializeField] List<Base_TreasureData> treasures;

    private void OnEnable()
    {
        int num_Options = Random.Range(3, 5);

        // ���̃��X�g�̃R�s�[���쐬
        List<Base_TreasureData> copyList = new List<Base_TreasureData>(treasures);

        for (int i = 0; i < num_Options; i++)
        {
            // �����ꃊ�X�g�̒��g����Ȃ璆�f
            if (copyList.Count <= 0) break;

            //�I�����ƂȂ��������
            Base_TreasureData y = copyList[Random.Range(0, copyList.Count)];

            // �{�^�����w���Transform���ɐ���
            var x = Instantiate(treasureButtonObj, buttonArea);

            // �{�^����I�΂ꂽ���ŏ�����
            x.GetComponent<Button_Treasure>().Initialize(y);

            // �R�s�[���ꂽ���X�g����I�΂ꂽ���̂��폜
            // �d����h������
            copyList.Remove(y);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
