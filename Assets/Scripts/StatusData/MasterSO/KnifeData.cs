using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewWeaponData", menuName = "Game Data/Weapon Data")]
public class KnifeData : ScriptableObject
{
    // �i�C�t�̊e�X�e�[�^�X���܂Ƃ߂�����
    // ���̃X�N���v�g��Unity�G�f�B�^��ɓo�^���邽�߂̂��̂ł���A�Q�[�����ł�_RunTime�����Ă���̂�����
    // �����̕ϐ��͊O������̎Q�ƂƁA�C���X�y�N�^�[��ł̏����������o����

    [field: SerializeField] public Sprite sprite { get; private set; }
    [field: SerializeField] public Sprite sprite_Defolme { get; private set; }
    [field: SerializeField] public Color color { get; private set; }

    [field: SerializeField] public string _name { get; private set; }

    [field: SerializeField] public int rarity { get; private set; }

    [field: SerializeField] public Element element { get; private set; }

    [field: SerializeField] public int power { get; private set; }
    [field: SerializeField] public int elementPower { get; private set; }

    [field: SerializeField] public GameObject prefab { get; private set; }

    [field: SerializeField] public GameObject hitEffect { get; private set; }

    // ��������ƃG�f�B�^��Œ����e�L�X�g�������₷���Ȃ�
    [Multiline(6)] public string description;

    // �������
    [field: SerializeField] public List<BaseHSpE> specialEffects { get; private set; }

    // �ȉ��L�[���[�h�\��
    /*
    public bool penetration; //�ђ�
    public bool defenceIgnore; // �h�䖳��
    public bool heavyBlow; // �d��
    public bool thighSplitting; // �ڍӂ�
    public bool collapse; // ����
    public bool blaze; // �Ή�
    public bool freeze; // �X��
    */
}
