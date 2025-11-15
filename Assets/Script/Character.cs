using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character : MonoBehaviour
{
    // (ตัวแปร Stats, Setup, Inventory ทั้งหมดเหมือนเดิม)
    [Header("Stats")]
    public float baseMovementSpeed = 5f;
    public int woodPlanks = 0;

    [Header("Setup")]
    public Transform weaponHolder; 
    
    [Header("Inventory")]
    public List<WeaponController> availableWeapons = new List<WeaponController>();
    public WeaponController currentWeapon;

    private float currentMovementSpeed;
    private GameObject currentInteractable; 

    void Start()
    {
        currentMovementSpeed = baseMovementSpeed;
    }

    void Update()
    {
        Move();
        HandleAiming();   // << ทำงานปกติ (แค่หมุนปืน)
        HandleAttack();   // << ทำงานปกติ (เรียก Fire() ใหม่)
        HandleInteract(); 
    }

    void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(moveX, moveY).normalized;
        transform.Translate(movement * currentMovementSpeed * Time.deltaTime);
    }

    // ฟังก์ชันนี้ "หมุนปืน" อย่างเดียว (ไม่เกี่ยวกับวิถีกระสุน)
    void HandleAiming()
    {
        if (currentWeapon != null)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = mousePos - weaponHolder.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            weaponHolder.rotation = Quaternion.Euler(0, 0, angle);

            SpriteRenderer weaponSprite = currentWeapon.GetComponent<SpriteRenderer>();
            if (weaponSprite != null)
            {
                weaponSprite.flipY = (Mathf.Abs(angle) > 90);
            }
        }
    }

    void HandleAttack()
    {
        // เรียก Fire() เวอร์ชันใหม่ (ที่ยิงกระสุนจริง)
        if (Input.GetButtonDown("Fire1") && currentWeapon != null)
        {
            bool shotFired = currentWeapon.Fire();
            if (!shotFired)
            {
                DestroyWeapon(currentWeapon);
            }
        }
    }

    // (ฟังก์ชันอื่นๆ: HandleInteract, ReceiveItem, AddWeapon, DestroyWeapon, SpeedBoost, Triggers... เหมือนเดิมทั้งหมด)
    
    // ... (คัดลอกส่วนที่เหลือของ Character.cs มาวางต่อที่นี่) ...
    // --- 4. การปฏิสัมพันธ์ (เก็บของ/ซ่อม) ---
    void HandleInteract()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            if (currentInteractable.CompareTag("LootBox"))
            {
                currentInteractable.GetComponent<LootBox>().Open(this);
            }
            else if (currentInteractable.CompareTag("Base"))
            {
                if (woodPlanks > 0)
                {
                    woodPlanks--;
                    currentInteractable.GetComponent<Base>().Repair(20);
                    Debug.Log("ซ่อมบ้าน! ไม้เหลือ: " + woodPlanks);
                }
            }
        }
    }

    // --- ระบบรับไอเทม (ถูกเรียกจาก LootBox) ---
    public void ReceiveItem(LootItemData item)
    {
        if (item is WeaponLootData)
        {
            WeaponLootData data = (WeaponLootData)item;
            AddWeapon(data.weaponPrefab, data.startingAmmo);
        }
        else if (item is PotionLootData)
        {
            PotionLootData data = (PotionLootData)item;
            StartCoroutine(SpeedBoostCoroutine(data.boostAmount, data.duration));
        }
        else if (item is PlankLootData)
        {
            PlankLootData data = (PlankLootData)item;
            woodPlanks += data.amount;
            Debug.Log("ได้ไม้เพิ่ม! รวม: " + woodPlanks);
        }
    }

    // --- ระบบจัดการปืน ---
    public void AddWeapon(GameObject weaponPrefab, int startingAmmo)
    {
        if (currentWeapon != null)
        {
            Destroy(currentWeapon.gameObject);
            availableWeapons.Clear();
        }

        GameObject newWeaponObj = Instantiate(weaponPrefab, weaponHolder.position, weaponHolder.rotation);
        newWeaponObj.transform.SetParent(weaponHolder); 
        newWeaponObj.transform.localPosition = Vector3.zero; 

        WeaponController newWeapon = newWeaponObj.GetComponent<WeaponController>();
        newWeapon.currentAmmo = startingAmmo;

        availableWeapons.Add(newWeapon);
        currentWeapon = newWeapon;
    }

    public void DestroyWeapon(WeaponController weapon)
    {
        availableWeapons.Remove(weapon);
        Destroy(weapon.gameObject);
        currentWeapon = null;
        Debug.Log("ปืนพัง! (กระสุนหมด)");
    }

    // --- ระบบยาเพิ่มความเร็ว ---
    IEnumerator SpeedBoostCoroutine(float boost, float duration)
    {
        currentMovementSpeed += boost;
        Debug.Log("วิ่งเร็วขึ้น!");
        yield return new WaitForSeconds(duration);
        currentMovementSpeed = baseMovementSpeed;
        Debug.Log("ความเร็วปกติ");
    }

    // --- ตรวจจับการชน (Trigger) ---
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("LootBox") || other.CompareTag("Base"))
        {
            currentInteractable = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == currentInteractable)
        {
            currentInteractable = null;
        }
    }
}