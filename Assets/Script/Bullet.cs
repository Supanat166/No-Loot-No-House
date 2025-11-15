using UnityEngine;

public class Bullet : MonoBehaviour
{
    // ตั้งค่ากระสุน (กำหนดใน Inspector ของ Prefab กระสุน)
    public float moveSpeed = 20f;
    public float damage = 50f;

    private Vector2 direction; // ทิศทางที่จะพุ่งไป

    void Start()
    {
        // ทำลายตัวเองทิ้งใน 5 วินาที (ถ้าไม่ชนอะไรเลย)
        Destroy(gameObject, 5f);
    }

    // ฟังก์ชันนี้จะถูกเรียกโดยปืน (WeaponController)
    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void Update()
    {
        // พุ่งไปข้างหน้าตามทิศทาง
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    // เมื่อชนอะไรบางอย่าง (ต้องติ๊ก Is Trigger)
    void OnTriggerEnter2D(Collider2D other)
    {
        // ถ้าชน "Monster"
        if (other.CompareTag("Monster"))
        {
            Monster monster = other.GetComponent<Monster>();
            if (monster != null)
            {
                monster.TakeDamage(damage); // ส่งดาเมจ
            }
            Destroy(gameObject); // ทำลายกระสุนทิ้ง
        }
        
    }
}