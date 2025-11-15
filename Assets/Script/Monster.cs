using UnityEngine;

public class Monster : MonoBehaviour
{
    // [เพิ่ม 1] สร้างช่องสำหรับลาก Prefab เอฟเฟคควันมาใส่
    public GameObject deathEffectPrefab; 

    public float health = 100f;
    public int damage = 15;
    public float moveSpeed = 1.5f;
    
    private Base targetBase;
    private Rigidbody2D rb;

    void Start()
    {
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Base"))
        {
            Base baseScript = other.GetComponent<Base>();
            if (baseScript != null)
            {
                baseScript.TakeDamage(damage);
            }
            
            // [เพิ่ม 2] สั่งสร้างเอฟเฟค ณ ตำแหน่งนี้ ก่อนตาย
            if (deathEffectPrefab != null)
            {
                Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
            }

            monstersAlive--; 
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            
            if (deathEffectPrefab != null)
            {
                Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
            }

            monstersAlive--; 
            Destroy(gameObject);
        }
    }

    // (อย่าลืมประกาศ public static int monstersAlive = 0; ไว้ด้วยนะครับ ถ้ายังไม่ได้เพิ่ม)
    public static int monstersAlive = 0; 
}