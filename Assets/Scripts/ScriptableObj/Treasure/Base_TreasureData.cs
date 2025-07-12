using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreasure", menuName = "Game Data/Treasure Data/zzzDont")]
public abstract class Base_TreasureData : ScriptableObject
{
    public string _name;
    [TextArea] public string _description;
    public Sprite icon;

    // �Q�b�g�������̏���
    public abstract void OnAdd(PlayerStatus status);

    // �폜���ꂽ���̏���
    public abstract void OnRemove(PlayerStatus status);

    // ����̃A�N�V�����ɔ������鏈��
    public abstract void SubscribeToEvent();
}
