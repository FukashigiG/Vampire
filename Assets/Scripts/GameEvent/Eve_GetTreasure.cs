using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

// ���ꁫ���L�q���Ă������ƂŁASystem��UnityEngine��Ranbom���������Ȃ��Ȃ�
using Random = UnityEngine.Random;
using Cysharp.Threading.Tasks;

public class Eve_GetTreasure : Base_EventCtrler
{
    [SerializeField] GameObject treasureButtonObj;

    List<Base_TreasureData> allTreasures;

    PlayerInventory playerInventory;

    public override void Initialize()
    {
        base.Initialize();

        playerInventory = PlayerController.Instance._status.inventory;

        // �Q�[���ɓo�^���ꂽ�S�Ă̔���z��Ƃ��Ď擾
        var x = Resources.LoadAll<Base_TreasureData>("GameDatas/Treasure");

        // �z���List�ɕϊ��Atreasures�Ɋi�[
        allTreasures = new List<Base_TreasureData>(x);
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        // PlayerInventory�̎�����ID��HashSet�Ɋi�[
        // HashSet���g�����ƂŁA��́u�܂܂�Ă邩�v�̃`�F�b�N���y�ɂȂ�
        HashSet<int> excludedIDs = new HashSet<int>(playerInventory.runtimeTreasure.Select(x => x.uniqueIP));

        // Player�̎����ĂȂ����̃��X�g���쐬
        List<Base_TreasureData> availableTreasures = allTreasures
            .Where(treasureAsset => !excludedIDs.Contains(treasureAsset.uniqueIP))
            .ToList();

        // ���X�g�̒��g���V���b�t��
        availableTreasures = availableTreasures.OrderBy(a => Guid.NewGuid()).ToList();

        // �I�����̑���������
        int num_Options = Random.Range(3, 5);

        for (int i = 0; i < num_Options; i++)
        {
            // �����ꃊ�X�g�̒��g����Ȃ璆�f
            if (availableTreasures.Count <= 0) break;

            //�I�����ƂȂ��������
            Base_TreasureData treasureData = availableTreasures[0];

            // �{�^�����w���Transform���ɐ���
            var buttonObj = Instantiate(treasureButtonObj, buttonArea);

            // �{�^����I�΂ꂽ���ŏ�����
            buttonObj.GetComponent<Button_Treasure>().Initialize(treasureData);

            // ���������{�^���������ꂽ�Ƃ��̏�����ǉ�
            buttonObj.GetComponent<Button>().onClick.AddListener(() =>
            {
                // �I�������I�΂ꂽ
                Choice(treasureData);
            });

            // �R�s�[���ꂽ���X�g����I�΂ꂽ���̂��폜
            // �d����h������
            availableTreasures.Remove(treasureData);
        }
    }

    void Choice(Base_TreasureData treasureData)
    {
        //�@�I�����ꂽ�����C���x���g���ɒǉ�
        playerInventory.AddTreasure(treasureData);

        //�p�l�������
        this.gameObject.SetActive(false);
    }
}
