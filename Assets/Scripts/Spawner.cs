using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;
    public SpawnData bossData;                  // 👈 보스 전용 데이터 추가
    public float levelTime;
    public int level;
    float timer;
    bool bossSpawned = false;                   // 👈 보스가 이미 소환되었는지 체크

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

        float remainTime = GameManager.instance.maxGameTime - GameManager.instance.gameTime;

        // 👇 남은 시간이 60초 이하일 경우, 보스만 1번 소환
        if (remainTime <= 60f)
        {
            if (!bossSpawned)
            {
                bossSpawned = true;
                SpawnBoss();
            }

            return; // 일반 몬스터 소환 중단
        }

        // 👇 일반 몬스터 소환 로직
        int cycleTime = 5;
        level = (int)(GameManager.instance.gameTime / cycleTime) % spawnData.Length;

        if (timer > spawnData[level].spawnTime)
        {
            timer = 0;
            Spawn();
        }
    }

    void Spawn()
    {
        GameObject enemy = GameManager.instance.pool.Get(0); // 일반 몬스터
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
    }

    void SpawnBoss()
    {
        GameObject boss = GameManager.instance.pool.Get(3); // 👈 보스는 prefabs[3]에 등록해야 함
        boss.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        boss.GetComponent<Enemy>().Init(bossData);
    }
}

[System.Serializable]
public class SpawnData
{
    public float spawnTime;
    public int spriteType;
    public int health;
    public float speed;
}
