using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAdmin : MonoBehaviour
{
    [SerializeField] int num_Wave;

    bool onGame;

    void Start()
    {
        onGame = true;

        GameProgression().Forget();
    }

    //�S�̓I�ȃQ�[���̐i�s���Ǘ�
    async UniTask GameProgression()
    {
        for (int i = 0; i < num_Wave; i++)
        {
            await UniTask.Delay((int)(5000));
        }

        onGame = false;
    }
}
