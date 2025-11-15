using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    public GameObject monsterPrefab;
    public List<Transform> spawnPoints; 

    public int waveNumber = 1;
    public int enemiesPerWave = 5;
    public float timeBetweenWaves = 10f; 

    void Start()
    {
        StartCoroutine(SpawnWaveCoroutine());
    }

    IEnumerator SpawnWaveCoroutine()
    {
        Debug.Log("เตรียมตัว! เวฟแรกจะเริ่มใน " + timeBetweenWaves + " วินาที...");
        yield return new WaitForSeconds(timeBetweenWaves);

        while (waveNumber <= 5)
        {
            Debug.Log("Wave " + waveNumber + " เริ่ม!");

            // (ส่วนสร้างซอมบี้ - เหมือนเดิม)
            for (int i = 0; i < enemiesPerWave; i++)
            {
                if (spawnPoints.Count > 0)
                {
                    int randomIndex = Random.Range(0, spawnPoints.Count);
                    Transform randomPoint = spawnPoints[randomIndex];
                    Instantiate(monsterPrefab, randomPoint.position, randomPoint.rotation);
                }
                yield return new WaitForSeconds(0.5f);
            }

            // --- [แก้ใหม่] ---
            // รอจนกว่าซอมบี้จะตายหมด (เหลือ 0)
            Debug.Log("Wave " + waveNumber + " รอเคลียร์ซอมบี้...");
            while (Monster.monstersAlive > 0)
            {
                yield return null; // "รอเฟรมถัดไป" แล้วเช็คใหม่
            }

            Debug.Log("Wave " + waveNumber + " เคลียร์!");
            // ------------------

            // ถ้าเคลียร์เวฟ 5 ได้แล้ว ก็จบเลย
            if (waveNumber == 5)
            {
                break;
            }

            // (พัก 10 วิ ก่อนเริ่มเวฟต่อไป)
            Debug.Log("พัก " + timeBetweenWaves + " วิ...");
            yield return new WaitForSeconds(timeBetweenWaves);

            waveNumber++;
            enemiesPerWave += 3;
        }

        // --- [แก้ใหม่] ---
        // (โค้ดนี้จะทำงาน "หลังจาก" เคลียร์เวฟ 5 (ซอมบี้ = 0) แล้ว)
        Debug.Log("สุดยอด! คุณรอดพ้นทั้ง 5 เวฟแล้ว! (YOU WIN)");
        
        // นี่คือคำสั่ง "หยุดเกม" ครับ
        Time.timeScale = 0f; 
        // -----------------
    }
}