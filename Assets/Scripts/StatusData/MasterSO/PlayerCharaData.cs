using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerCharaData", menuName = "Game Data/Player Chara Data")]
public class PlayerCharaData : ScriptableObject
{
    // �v���C���[�L�����N�^�[�̊e�X�e�[�^�X���܂Ƃ߂�����
    // ���̃X�N���v�g��Unity�G�f�B�^��ɓo�^���邽�߂̂��̂ł���A�Q�[�����ł�_RunTime�����Ă���̂�����
    // �����̕ϐ��͊O������̎Q�ƂƁA�C���X�y�N�^�[��ł̏����������o����

    [field: SerializeField] public Sprite sprite { get; private set; }

    [field: SerializeField] public string _name { get; private set; }

    [field: SerializeField] public int hp { get; private set; }
    [field: SerializeField] public int power { get; private set; }
    [field: SerializeField] public int defense { get; private set; }
    [field: SerializeField] public int moveSpeed { get; private set; }
    [field: SerializeField] public float luck { get; private set; }
    [field: SerializeField] public float eyeSight { get; private set; }

    [field: SerializeField] public KnifeData[] initialKnives { get; private set; }
    [field: SerializeField] public Base_TreasureData[] initialTreasures { get; private set; }

    [field: SerializeField] public Element masteredElement { get; private set; }

    [field: SerializeField] public GameObject prefab { get; private set; }
}
