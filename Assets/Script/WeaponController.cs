using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Settings")]
    public int currentAmmo;     
    public Transform firePoint; 

    [Header("Bullet")]
    public GameObject bulletPrefab; 
    
    public bool Fire()
    {
        if (currentAmmo > 0)
        {
            currentAmmo--;

            // 1. หาตำแหน่งเมาส์
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            // 2. หาทิศทางจาก "ปลายปืน" พุ่งไปหา "เมาส์"
            Vector2 shootDirection = (mousePos - firePoint.position).normalized;

            // 3. "สร้าง" (Instantiate) กระสุนขึ้นมา
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

            // 4. บอกกระสุนว่าให้พุ่งไปทางไหน
            bullet.GetComponent<Bullet>().SetDirection(shootDirection);

            return true; 
        }
        else
        {
            return false; 
        }
    }
}