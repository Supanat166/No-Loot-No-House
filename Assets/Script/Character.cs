using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character : MonoBehaviour
{
    // === STATS ===
    [Header("Stats")]
    public float baseMovementSpeed = 5f; //ความเร็วเริ่มต้นของผู้เล่น
    public int woodPlanks = 0;           //จำนวนไม้
    public int woodCostPerRepair = 5;   // ใช้ไม้กี่แผ่นต่อการซ่อม 1 ครั้ง
    public int totalAmmo = 0;
    
    // === SETUP ===
    [Header("Setup")]
    public Transform weaponHolder; // จุดหมุนปืน (ต้องลาก Empty Object ที่มือมาใส่)

    // === INVENTORY ===
    [Header("Inventory")]
    public WeaponController currentWeapon;
    public int potionCount = 0; // จำนวนยาที่เก็บได้
    private PotionLootData storedPotionData; // "พิมพ์เขียว" ยาที่เก็บไว้ (เอาไว้รู้ค่า boost/duration)

    // === AUDIO ===
    [Header("Audio Clips")]
    public AudioClip shootSound;
    public AudioClip potionSound;
    public AudioClip repairSound;

    // === Private Variables ===
    private AudioSource audioSource; // "ลำโพง" ที่ติดตัวผู้เล่น
    private float currentMovementSpeed;
    private GameObject currentInteractable; // ของที่อยู่ใกล้ๆ (กล่อง/บ้าน)
    private SpriteRenderer playerSprite;

    //==================================================================
    //  UNITY FUNCTIONS (Start, Update)
    //==================================================================

    void Start()
    {
        currentMovementSpeed = baseMovementSpeed;
        playerSprite = GetComponent<SpriteRenderer>();

        // ดึง "ลำโพง" มาเก็บไว้ (ต้อง Add Component "Audio Source" ที่ Player ก่อน)
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.playOnAwake = false; // ปิดเล่นเสียงอัตโนมัติตอนเริ่ม
        }
    }

    void Update()
    {
        Move();
        HandleAiming();
        HandleAttack();
        HandleInteract();
        HandleUsePotion(); // เช็คการกด R ใช้ยา
    }

    //==================================================================
    //  CORE MECHANICS (Movement, Aiming, Attack, Interact)
    //==================================================================

    void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        
        if (playerSprite != null)
        {
            if (moveX > 0) 
            {
                playerSprite.flipX = false; 
            }
            else if (moveX < 0) 
            {
                playerSprite.flipX = true;  
            }
        }

        Vector2 movement = new Vector2(moveX, moveY).normalized;
        transform.Translate(movement * currentMovementSpeed * Time.deltaTime);
    }

    void HandleAiming()
    {
        if (currentWeapon != null && weaponHolder != null)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = mousePos - weaponHolder.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            weaponHolder.rotation = Quaternion.Euler(0, 0, angle);

            // พลิกปืนไม่ให้กลับหัว
            SpriteRenderer weaponSprite = currentWeapon.GetComponent<SpriteRenderer>();
            if (weaponSprite != null)
            {
                weaponSprite.flipY = (Mathf.Abs(angle) > 90);
            }
        }
    }

    void HandleAttack()
    {
        if (Input.GetButtonDown("Fire1") && currentWeapon != null)
        {
            // เรียก Fire() โดยส่ง totalAmmo เข้าไป
            int ammoUsed = currentWeapon.Fire(totalAmmo);

            if (ammoUsed > 0)
            {
                totalAmmo -= ammoUsed; // ลดกระสุนตามจำนวนที่ยิงได้

                // เล่นเสียงยิง
                if (audioSource != null && shootSound != null)
                {
                    audioSource.PlayOneShot(shootSound);
                }
            }
            else
            {
                // กระสุนหมด
                Debug.Log("ยิงไม่ได้: กระสุนหมด");
                DestroyWeapon(currentWeapon);
            }
        }
    }

    void HandleInteract()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            if (currentInteractable.CompareTag("LootBox"))
            {
                LootBox box = currentInteractable.GetComponent<LootBox>();
                if (box != null)
                    box.Open(this);
            }
            else if (currentInteractable.CompareTag("Base"))
            {
                Base baseObj = currentInteractable.GetComponent<Base>();
                if (baseObj != null)
                {
                    if (woodPlanks >= woodCostPerRepair)
                    {
                        woodPlanks -= woodCostPerRepair;
                        baseObj.Repair(20);

                        // เล่นเสียงซ่อม
                        if (audioSource != null && repairSound != null)
                        {
                            audioSource.PlayOneShot(repairSound);
                        }

                        Debug.Log($"ซ่อมบ้าน! ใช้ไม้ {woodCostPerRepair} ชิ้น เหลือ: {woodPlanks}");
                    }
                    else
                    {
                        Debug.Log($"ไม้ไม่พอซ่อมบ้าน! ต้องใช้ {woodCostPerRepair} แต่มี {woodPlanks}");
                    }
                }
            }
        }
    }

    //==================================================================
    //  ITEM & BUFF SYSTEM
    //==================================================================

    void HandleUsePotion()
    {
        if (Input.GetKeyDown(KeyCode.R) && potionCount > 0 && currentMovementSpeed <= baseMovementSpeed)
        {
            potionCount--;

            // เล่นเสียงดื่มยา
            if (audioSource != null && potionSound != null)
            {
                audioSource.PlayOneShot(potionSound);
            }

            if (storedPotionData != null)
            {
                StartCoroutine(SpeedBoostCoroutine(storedPotionData.boostAmount, storedPotionData.duration));
            }
            else
            {
                Debug.LogWarning("กดใช้ยา แต่ storedPotionData เป็น null");
            }

            Debug.Log("ใช้ยา! เหลือ: " + potionCount);

            if (potionCount == 0)
            {
                storedPotionData = null;
            }
        }
    }

    // ฟังก์ชันนี้ถูกเรียกจาก LootBox.Open(this);
    public void ReceiveItem(LootItemData item)
    {
        if (item == null)
        {
            Debug.LogWarning("ReceiveItem ถูกเรียกด้วย item = null");
            return;
        }

        // 1) ได้ปืน
        WeaponLootData weaponLoot = item as WeaponLootData;
        if (weaponLoot != null)
        {
            // เพิ่มกระสุนให้กับ totalAmmo โดยตรง
            totalAmmo += weaponLoot.startingAmmo;

            // เปลี่ยนปืนใหม่
            AddWeapon(weaponLoot.weaponPrefab); 

            Debug.Log($"ได้ปืนใหม่! เพิ่มกระสุน +{weaponLoot.startingAmmo} นัด, กระสุนรวม: {totalAmmo}");

            return;
        
        }

        // 2) ได้ Potion
        PotionLootData potionLoot = item as PotionLootData;
        if (potionLoot != null)
        {
            storedPotionData = potionLoot;
            potionCount++;   // ถ้า PotionLootData ไม่มี amount ก็เพิ่มทีละ 1
            Debug.Log($"ได้ยา! ตอนนี้มียารวม: {potionCount}");
            return;
        }

        // 3) ได้ไม้
        PlankLootData plankLoot = item as PlankLootData;
        if (plankLoot != null)
        {
            woodPlanks += plankLoot.amount;   
            Debug.Log($"ได้ไม้ +{plankLoot.amount} แผ่น รวม: {woodPlanks}");
            return;
        }

        Debug.LogWarning("ReceiveItem: ยังไม่ได้รองรับไอเท็มชนิดนี้ -> " + item.name);
    }

    IEnumerator SpeedBoostCoroutine(float boost, float duration)
    {
        currentMovementSpeed += boost;
        Debug.Log("วิ่งเร็วขึ้น! +" + boost);
        yield return new WaitForSeconds(duration);
        currentMovementSpeed = baseMovementSpeed;
        Debug.Log("ความเร็วกลับสู่ปกติ");
    }

    //==================================================================
    //  WEAPON MANAGEMENT
    //==================================================================

    public void AddWeapon(GameObject weaponPrefab) 
    {
        if (weaponPrefab == null)
        {
            Debug.LogWarning("AddWeapon ถูกเรียก แต่ weaponPrefab = null");
            return;
        }

        if (currentWeapon != null)
        {
            // ทำลายปืนเก่า
            Destroy(currentWeapon.gameObject);
            
        }

        GameObject newWeaponObj = Instantiate(weaponPrefab, weaponHolder.position, weaponHolder.rotation);
        newWeaponObj.transform.SetParent(weaponHolder);
        newWeaponObj.transform.localPosition = Vector3.zero;

        WeaponController newWeapon = newWeaponObj.GetComponent<WeaponController>();
        if (newWeapon == null)
        {
            Debug.LogError("WeaponPrefab ไม่มี Component WeaponController");
            return;
        }

        
        currentWeapon = newWeapon;
    }

    public void DestroyWeapon(WeaponController weapon)
    {
        if (weapon == null) return;

        
        Destroy(weapon.gameObject);
        currentWeapon = null;
        Debug.Log("ปืนถูกทำลาย/เปลี่ยน");
    }

    //==================================================================
    //  PHYSICS TRIGGERS
    //==================================================================

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


