using UnityEngine;

public class Monster : MonoBehaviour
{
    public GameObject deathEffectPrefab; 
    public HealthBarUI healthBarUI;
    public float health = 100f;
    private float maxhealth = 0f;
    public int damage = 15;
    public float moveSpeed = 1.5f;
    
    [Header("Audio")]
    public AudioClip deathSound; 
    
    private Base targetBase;
    private Rigidbody2D rb;

    void Start()
    {
        maxhealth = health;
        healthBarUI.slider.maxValue=maxhealth;
        healthBarUI.slider.value=health;
        targetBase = FindObjectOfType<Base>();
        rb = GetComponent<Rigidbody2D>();
        monstersAlive++;
        Debug.Log("ซอมบี้เกิด! ตอนนี้มี: " + monstersAlive);
    }

    void FixedUpdate()
    {
        if (targetBase != null)
        {
            Vector2 newPos = Vector2.MoveTowards(rb.position, targetBase.transform.position, moveSpeed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);
        }
    }

    // --- [แก้ไข] ---
    // เมื่อชนบ้าน ให้เรียก Die()
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Base"))
        {
            Base baseScript = other.GetComponent<Base>();
            if (baseScript != null)
            {
                baseScript.TakeDamage(damage);
            }
            
            Die(); // เรียกฟังก์ชันตาย
        }
    }

    // --- [แก้ไข] ---
    // เมื่อโดนยิง แล้วเลือดหมด ให้เรียก Die()
    public void TakeDamage(float amount)
    {
        health -= amount;
        healthBarUI.slider.value=health;
        if (health <= 0)
        {
            Die(); // เรียกฟังก์ชันตาย
        }
    }

    // --- [เพิ่ม] ---
    // สร้างฟังก์ชัน Die() เพื่อรวมทุกอย่างที่ต้องทำตอนตาย
    void Die()
    {
        // 1. ลดตัวนับ (ต้องทำก่อน Destroy)
        monstersAlive--; 
        Debug.Log("ซอมบี้ตาย! เหลือ: " + monstersAlive);

        // 2. สร้างเอฟเฟคควัน
        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }

        // 3. เล่นเสียงตาย
        if (deathSound != null)
        {
            // สร้างเสียง ณ จุดที่ตาย (วิธีนี้ดีที่สุดเพราะตัวซอมบี้กำลังจะหายไป)
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
        }

        // 4. ทำลายตัวเอง
        Destroy(gameObject);
    }

    
    public static int monstersAlive = 0; 
}