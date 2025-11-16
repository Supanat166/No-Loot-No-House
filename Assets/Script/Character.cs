using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character : MonoBehaviour
{
    // === STATS ===
    [Header("Stats")]
    public float baseMovementSpeed = 5f;
    public int woodPlanks = 0;

    // === SETUP ===
    [Header("Setup")]
    public Transform weaponHolder; // จุดหมุนปืน (ต้องลาก Empty Object ที่มือมาใส่)
    
    // === INVENTORY ===
    [Header("Inventory")]
    public List<WeaponController> availableWeapons = new List<WeaponController>();
    public WeaponController currentWeapon;
    public int potionCount = 0; // จำนวนยาที่เก็บได้
    private PotionLootData storedPotionData; // "พิมพ์เขียว" (ข้อมูล) ของยาที่เก็บไว้

    // === AUDIO ===
    [Header("Audio Clips")]
    public AudioClip shootSound;
    public AudioClip potionSound;
    public AudioClip repairSound;

    // === Private Variables ===
    private AudioSource audioSource; // "ลำโพง" ที่ติดตัวผู้เล่น
    private float currentMovementSpeed;
    private GameObject currentInteractable; // ของที่อยู่ใกล้ๆ (กล่อง/บ้าน)

    //==================================================================
    //  UNITY FUNCTIONS (Start, Update)
    //==================================================================

    void Start()
    {
        currentMovementSpeed = baseMovementSpeed;
        
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

        Vector2 movement = new Vector2(moveX, moveY).normalized;
        transform.Translate(movement * currentMovementSpeed * Time.deltaTime);
    }

    void HandleAiming()
    {
        if (currentWeapon != null)
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
            bool shotFired = currentWeapon.Fire();
            if (shotFired)
            {
                // เล่นเสียงยิง
                if (audioSource != null && shootSound != null)
                {
                    audioSource.PlayOneShot(shootSound);
                }
            }
            else
            {
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
                currentInteractable.GetComponent<LootBox>().Open(this);
            }
            else if (currentInteractable.CompareTag("Base"))
            {
                if (woodPlanks > 0)
                {
                    woodPlanks--;
                    currentInteractable.GetComponent<Base>().Repair(20);
                    
                    // เล่นเสียงซ่อม
                    if (audioSource != null && repairSound != null)
                    {
                        audioSource.PlayOneShot(repairSound);
                    }
                    Debug.Log("ซ่อมบ้าน! ไม้เหลือ: " + woodPlanks);
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
            
            StartCoroutine(SpeedBoostCoroutine(storedPotionData.boostAmount, storedPotionData.duration));
            Debug.Log("ใช้ยา! เหลือ: " + potionCount);

            if (potionCount == 0)
            {
                storedPotionData = null;
            }
        }
    }

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
            storedPotionData = data; // เก็บ "พิมพ์เขียว" ยา
            potionCount++;           // เพิ่ม "จำนวน" ยา
            Debug.Log("เก็บยาได้! มีทั้งหมด: " + potionCount);
        }
        else if (item is PlankLootData)
        {
            PlankLootData data = (PlankLootData)item;
            woodPlanks += data.amount;
            Debug.Log("ได้ไม้เพิ่ม! รวม: " + woodPlanks);
        }
    }

    IEnumerator SpeedBoostCoroutine(float boost, float duration)
    {
        currentMovementSpeed += boost;
        Debug.Log("วิ่งเร็วขึ้น!");
        yield return new WaitForSeconds(duration);
        currentMovementSpeed = baseMovementSpeed;
        Debug.Log("ความเร็วปกติ");
    }

    //==================================================================
    //  WEAPON MANAGEMENT
    //==================================================================

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
        Debug.Log("ปืนพัง! (กระSunหมด)");
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