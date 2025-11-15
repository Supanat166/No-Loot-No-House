using UnityEngine;

public class Base : MonoBehaviour
{
    public int maxHealth = 300;
    public int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        // [TODO] อัปเดต UI หลอดเลือดบ้าน
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0;
        
        // [TODO] อัปเดต UI หลอดเลือดบ้าน
        Debug.Log("Base HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Repair(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        
        // [TODO] อัปเดต UI หลอดเลือดบ้าน
        Debug.Log("ซ่อมบ้านแล้ว! HP: " + currentHealth);
    }

    void Die()
    {
        Debug.Log("GAME OVER - บ้านพัง!");
        // [TODO] แสดงหน้าจอ Game Over
        Time.timeScale = 0f; // หยุดเกม
    }
}