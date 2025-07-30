using System;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public int PoolId = 0;
    public float SpawnRadius = 5f;
    public Transform CircleCenter;
    public SpawnData[] SpawnDataArray;
    int level = 0;
    float timer = 0f;

    void Update()
    {
        if (!GameManager.Instance.IsLive)
            return;

        timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(GameManager.Instance.GameTime / 10f), SpawnDataArray.Length - 1);

        if (timer > SpawnDataArray[level].SpawnTime)
        {
            timer = 0;
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        Vector2 spawnPos = GetRandomPointOnCircle();
        GameObject enemy = GameManager.Instance.PoolManager.GetObject(0);

        enemy.transform.position = spawnPos;
        enemy.GetComponent<Enemy>().Init(SpawnDataArray[level]);
        enemy.SetActive(true);
    }

    Vector2 GetRandomPointOnCircle()
    {
        float angle = UnityEngine.Random.Range(0f, 2f * Mathf.PI);
        Vector2 center = CircleCenter.position;
        return center + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * SpawnRadius;
    }
}

[Serializable]
public class SpawnData
{
    public int SpawnType;
    public float SpawnTime;
    public float Health;
    public float Speed;
}