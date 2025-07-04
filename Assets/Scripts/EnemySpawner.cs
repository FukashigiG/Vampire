using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemy;
    [SerializeField] GameObject bossEnemy;

    [SerializeField] float interval_Spawn;

    void Start()
    {
        SpawnTask().Forget();
    }

    async UniTask SpawnTask()
    {
        while (true)
        {
            Vector2 randomPoint = Random.insideUnitCircle * 15;

            Instantiate(enemy, randomPoint, Quaternion.identity);

            await UniTask.Delay((int)(interval_Spawn * 1000));
        }
    }

    public GameObject SpawnBoss()
    {
        Vector2 randomPoint = Random.insideUnitCircle * 15;

        var x = Instantiate(bossEnemy, randomPoint, Quaternion.identity);

        return x;
    }
}
