using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;
    public float levelTime;

    public int level;
    float timer;

    void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
        levelTime = GameManager.instance.maxGameTime / spawnData.Length;
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        timer += Time.deltaTime;
        // 몬스터가 전체 게임 시간에 몬스터 수 만큼 나옴
        // level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / levelTime), spawnData.Length - 1);
        // 새 코드
        int cycleTime = 5; // 몬스터가 바뀌는 주기(초)
        level = (int)(GameManager.instance.gameTime / cycleTime) % spawnData.Length;


        if (timer > spawnData[level].spawnTime)
        {
            timer = 0;
            Spawn();
        }
    }


    void Spawn()
    {
        GameObject enemy = GameManager.instance.pool.Get(0);
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
    }
}

// .. 직렬화
[System.Serializable]
public class SpawnData
{
    public float spawnTime;
    public int spriteType;
    
    public int health;
    public float speed;
}