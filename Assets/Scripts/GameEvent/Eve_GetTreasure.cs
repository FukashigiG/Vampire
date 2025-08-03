using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

// ���ꁫ���L�q���Ă������ƂŁASystem��UnityEngine��Ranbom���������Ȃ��Ȃ�
using Random = UnityEngine.Random;

public class Eve_GetTreasure : Base_EventCtrler
{
    [SerializeField] GameObject treasureButtonObj;

    [SerializeField] Transform buttonArea;

    List<Base_TreasureData> allTreasures;

    public override void Initialize()
    {
        base.Initialize();

        // �Q�[���ɓo�^���ꂽ�S�Ă̔���z��Ƃ��Ď擾
        var x = Resources.LoadAll<Base_TreasureData>("GameDatas/Treasure");

        // �z���List�ɕϊ��Atreasures�Ɋi�[
        allTreasures = new List<Base_TreasureData>(x);
    }

    private void OnEnable()
    {
        // PlayerInventory�̎�����ID��HashSet�Ɋi�[
        // HashSet���g�����ƂŁA��́u�܂܂�Ă邩�v�̃`�F�b�N���y�ɂȂ�
        HashSet<int> excludedIDs = new HashSet<int>();

        foreach (var treasure in PlayerController.Instance._status.inventory.runtimeTreasure)
        {
            excludedIDs.Add(treasure.uniqueIP);
        }

        // Player�̎����ĂȂ����̃��X�g���쐬
        List<Base_TreasureData> availableTreasures = allTreasures
            .Where(treasureAsset => !excludedIDs.Contains(treasureAsset.uniqueIP))
            .ToList();

        // ���X�g�̒��g���V���b�t��
        availableTreasures = availableTreasures.OrderBy(a => Guid.NewGuid()).ToList();

        int num_Options = Random.Range(3, 5);

        for (int i = 0; i < num_Options; i++)
        {
            // �����ꃊ�X�g�̒��g����Ȃ璆�f
            if (availableTreasures.Count <= 0) break;

            //�I�����ƂȂ��������
            Base_TreasureData y = availableTreasures[0];

            // �{�^�����w���Transform���ɐ���
            var x = Instantiate(treasureButtonObj, buttonArea);

            // �{�^����I�΂ꂽ���ŏ�����
            x.GetComponent<Button_Treasure>().Initialize(y);

            // �R�s�[���ꂽ���X�g����I�΂ꂽ���̂��폜
            // �d����h������
            availableTreasures.Remove(y);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
