using UnityEngine;

[CreateAssetMenu(fileName ="NewWeaponData", menuName = "Game Data/Weapon Data")]
public class KnifeData : ScriptableObject
{
    public enum AttributeEnum
    {
        white, red, blue, yellow
    }

    public Sprite sprite;

    public string _name;

    public int rarity;

    public AttributeEnum attribute;

    public int power;

    public GameObject prefab;

    // ��������ƃG�f�B�^��Œ����e�L�X�g�������₷���Ȃ�
    [Multiline(6)] public string description;

    // �������
    public BaseHSpE[] specialEffects;

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
