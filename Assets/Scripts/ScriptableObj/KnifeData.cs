using UnityEngine;

[CreateAssetMenu(fileName ="NewWeaponData", menuName = "Game Data/Weapon Data")]
public class KnifeData : ScriptableObject
{
    public Sprite sprite;

    public string _name;

    public int rarity;

    public int power;

    public GameObject prefab;

    // ��������ƃG�f�B�^��Œ����e�L�X�g�������₷���Ȃ�
    [Multiline(6)] public string description;
}
