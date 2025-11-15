using UnityEngine;
using System.Collections.Generic;

public class ItemSpawn : MonoBehaviour
{
    public GameObject lootBoxPrefab; // Prefab ของ "กล่อง"
    public List<Transform> spawnPoints; // ลิสต์จุดเกิด (Empty Objects)
    public float spawnRate = 15f; // เกิดใหม่ทุก 15 วิ

    private float nextSpawnTime;

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnLootBox();
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    void SpawnLootBox()
    {
        if (lootBoxPrefab == null || spawnPoints.Count == 0) return;

        int spawnPointIndex = Random.Range(0, spawnPoints.Count);
        Transform point = spawnPoints[spawnPointIndex];

        Instantiate(lootBoxPrefab, point.position, point.rotation);
    }
}