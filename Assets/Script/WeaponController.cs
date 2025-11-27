using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Settings")]
    // ลบ public int currentAmmo; ออก
    public Transform firePoint;

    [Header("Bullet")]
    public GameObject bulletPrefab;

    // เปลี่ยน Fire() ให้รับ currentAmmo และส่งคืนกระสุนที่ใช้ไป
    // int currentAmmo: กระสุนปัจจุบัน
    // return int: จำนวนกระสุนที่ถูกใช้ไป (1 ถ้ามีการยิง, 0 ถ้าไม่มีการยิง)
    public int Fire(int currentAmmo)
    {
        if (currentAmmo > 0)
        {
            // ไม่ต้องลดกระสุนที่นี่แล้ว

            // 1. หาตำแหน่งเมาส์
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            // 2. หาทิศทางจาก "ปลายปืน" พุ่งไปหา "เมาส์"
            Vector2 shootDirection = (mousePos - firePoint.position).normalized;

            // 3. "สร้าง" (Instantiate) กระสุนขึ้นมา
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

            // 4. บอกกระสุนว่าให้พุ่งไปทางไหน
            bullet.GetComponent<Bullet>().SetDirection(shootDirection);

            return 1; // ยิงสำเร็จ, ใช้กระสุนไป 1 นัด
        }
        else
        {
            return 0; // ยิงไม่สำเร็จ, ใช้กระสุนไป 0 นัด
        }
    }
}