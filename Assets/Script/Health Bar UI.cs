using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public GameObject bloodEffectPrefab;   // ลาก Prefab เลือดมาใส่ใน Inspector

    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    // ฟังก์ชันเอาไว้เรียกเวลาตัวนี้โดนโจมตี
    public void TakeDamage(int damage, Vector2 hitPosition)
    {
        currentHealth -= damage;

        // สร้างเอฟเฟกต์เลือดตรงจุดที่โดนตี
        if (bloodEffectPrefab != null)
        {
            Instantiate(bloodEffectPrefab, hitPosition, Quaternion.identity);
        }

        // ถ้าเลือดหมด
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // ทำอะไรก็ได้ตอนตาย เช่น เล่นอนิเมชั่น / ลบตัวละคร
        Destroy(gameObject);
    }
}